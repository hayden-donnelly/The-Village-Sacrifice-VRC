using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMeshSurf : UdonSharpBehaviour
{
    [SerializeField] private int agentTypeID;
    [SerializeField] private Mesh mesh;

    public NavMeshBuildSettings GetBuildSettings()
    {
        NavMeshBuildSettings buildSettings = NavMesh.GetSettingsByID(agentTypeID);
        return buildSettings;
    }

    /*public void BuildNavMesh()
    {
        //NavMeshBuildSource[] sources = new NavMeshBuildSource[10];
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        Bounds sourcesBounds = mesh.bounds;

        NavMeshData data = NavMeshBuilder.BuildNavMeshData(GetBuildSettings(), 
            sources, sourcesBounds, transform.position, transform.rotation);
    }*/

    private void Start()
    {
        //BuildNavMesh();
    }
}
