using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.Collections;

public class LightLevelCalculator : UdonSharpBehaviour
{
    public float lightLevel;
    [SerializeField] private RenderTexture lightTexture;
    [SerializeField] private Texture2D tex;

    private void OnPostRender()
    {
        //CalculateLightLevel();
    }

    public float CalculateLightLevel()
    {
        Rect rectReadPicture = new Rect(0, 0, 24, 24);
        //RenderTexture.active = lightTexture;
        tex.ReadPixels(rectReadPicture, 0, 0, false);

        Vector4 topLeft = tex.GetPixels(0, 0, 1, 1, 0)[0];
        Vector4 topRight = tex.GetPixels(24, 0, 1, 1, 0)[0];
        Vector4 bottomLeft = tex.GetPixels(0, 24, 1, 1, 0)[0];
        Vector4 bottomRight = tex.GetPixels(24, 24, 1, 1, 0)[0];

        float lightLevelSum = topLeft.magnitude + topRight.magnitude 
                            + bottomLeft.magnitude + bottomRight.magnitude;


        Debug.Log(lightLevelSum);
        return lightLevelSum / 4;
    }
}
