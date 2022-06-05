using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.AI;

public class NavMeshSurf : UdonSharpBehaviour
{
    private void Start()
    {
        NavMeshBuilder.BuildNavMeshData();
    }
}
