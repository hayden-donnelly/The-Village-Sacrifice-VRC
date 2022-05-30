using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;
using UnityEngine.AI;

public class WorldGeneration : UdonSharpBehaviour
{
    [SerializeField] private double worldSeed;
    [SerializeField] private float interpolationWeight;
    [SerializeField] private GameObject redTile;
    [SerializeField] private GameObject greenTile;
    [SerializeField] private GameObject blueTile;
    [SerializeField] private Transform startingPoint1;
    [SerializeField] private Transform startingPoint2;
    [SerializeField] private GameObject tile1;
    [SerializeField] private GameObject tile2;
    [SerializeField] private GameObject tile3;
    //[SerializeField] private NavMeshSurface[][] surfaces = new NavMeshSurface[50][];
    public NavMeshSurface asd;

    private void Start()
    {
        ColorGridDemo();
        GenerateMaze();
    }

    private double GenerateNumber(double x, double seed)
    {   
        // I came up with this by messing around in Desmos.
        double number = 3;
        number *= Math.Cos(x + seed);
        number *= Math.Sin(x - 1 + 1/seed);
        number *= Math.Cos(x + 20);
        number *= Math.Sin(x - 20);
        number *= Math.Sin(x + 4);
        return number;
    }

    private Vector2 UnitGradientVector(Vector2 gridPoint, double seed)
    {
        Vector2 gradientVector = new Vector2((float)GenerateNumber((double)gridPoint.x, seed),
                                             (float)GenerateNumber((double)gridPoint.y, seed));
        return gradientVector.normalized;
    }

    private float Interpolate(float value1, float value2, float weight)
    {
        return value1 + weight * (value2 - value1);
    }

    private float PerlinNoise(Vector2 point, float cell_size, int grid_width, int grid_height)
    {
        // Grid points
        Vector2 bottomLeftGridPoint = new Vector2((float)Math.Floor(point.x / cell_size) * cell_size, 
                                                  (float)Math.Floor(point.y / cell_size) * cell_size);
        Vector2 bottomRightGridPoint = new Vector2(bottomLeftGridPoint.x + cell_size, bottomLeftGridPoint.y);
        Vector2 topLeftGridPoint = new Vector2(bottomLeftGridPoint.x, bottomLeftGridPoint.y + cell_size);
        Vector2 topRightGridPoint = new Vector2(bottomLeftGridPoint.x + cell_size, bottomLeftGridPoint.y + cell_size);

        // Offset vectors
        Vector2 bottomLeftOffset = point - bottomLeftGridPoint;
        Vector2 bottomRightOffset = point - bottomRightGridPoint;
        Vector2 topLeftOffset = point - topLeftGridPoint;
        Vector2 topRightOffset = point - topRightGridPoint;

        // Unit gradient vectors
        Vector2 bottomLeftUnitGradient = UnitGradientVector(bottomLeftGridPoint, worldSeed);
        Vector2 bottomRightUnitGradient = UnitGradientVector(bottomRightGridPoint, worldSeed);
        Vector2 topLeftUnitGradient = UnitGradientVector(topLeftGridPoint, worldSeed);
        Vector2 topRightUnitGradient = UnitGradientVector(topRightGridPoint, worldSeed);

        // Dot products
        float bottomLeftDot = Vector2.Dot(bottomLeftUnitGradient, bottomLeftOffset);
        float bottomRightDot = Vector2.Dot(bottomRightUnitGradient, bottomRightOffset);
        float topLeftDot = Vector2.Dot(topLeftUnitGradient, topLeftOffset);
        float topRightDot = Vector2.Dot(topRightUnitGradient, topRightOffset);

        // Interpolate
        float firstInterpolation = Interpolate(bottomLeftDot, bottomRightDot, interpolationWeight);
        float secondInterpolation = Interpolate(topLeftDot, topRightDot, interpolationWeight);
        float finalInterpolation = Interpolate(firstInterpolation, secondInterpolation, interpolationWeight);

        return finalInterpolation;
    }

    private void ColorGridDemo()
    {
        for(int x = 0; x < 50; x++)
        {
            for(int y = 0; y < 50; y++)
            {
                GameObject tile;
                float noise = PerlinNoise(new Vector2(x, y), 1.2f, 100, 100);
                if(noise < -2)
                {
                    tile = redTile;
                }
                else if(noise > 2)
                {
                    tile = blueTile;
                }
                else
                {
                    Debug.Log("fuck");
                    tile = greenTile;
                }
                Instantiate(tile, startingPoint1.position + new Vector3(x, y, 0), startingPoint1.rotation);
            }
        }
    }

    private void GenerateMaze()
    {
        for(int x = 0; x < 50; x++)
        {
            for(int y = 0; y < 50; y++)
            {
                GameObject tile;
                float noise = PerlinNoise(new Vector2(x, y), 1.2f, 100, 100);
                if(noise < -2)
                {
                    tile = tile1;
                }
                else if(noise > 5)
                {
                    tile = tile2;
                }
                else
                {
                    tile = tile3;
                }
                //surfaces[x,y] = (NavMeshSurface)Instantiate(tile, startingPoint2.position + new Vector3(-x*16, 0, y*16), startingPoint2.rotation);
                //surfaces[x,y].BuildNavMesh();
            }
        }
    }

    private void Gen()
    {
        // Map player position onto noise grid.
        // Calculate noise in a square around the player.
        // Instantiate maze tiles based on calculated noise.
        
        int squareLength = 50;
        Vector3 playerPosition = Networking.LocalPlayer.GetPosition();
        Vector3 startingPosition = new Vector3(playerPosition.x - squareLength/2, 0, playerPosition.z - squareLength/2);

        for(int i = 0; i < squareLength; i++)
        {
            for(int j = 0; j < squareLength; j++)
            {
                GameObject tile;
                float noise = PerlinNoise(new Vector2(playerPosition.x/100, playerPosition.z/100), 5f, 100, 100);
                if(noise < -2)
                {
                    tile = tile1;
                }
                else if(noise > 5)
                {
                    tile = tile2;
                }
                else
                {
                    tile = tile3;
                }
                Instantiate(tile, startingPosition + new Vector3(i*16, 0, j*16), startingPoint2.rotation);
            }
        }
    }
}
