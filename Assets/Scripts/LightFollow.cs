using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LightFollow : UdonSharpBehaviour
{
    private VRCPlayerApi localPlayer;

    private void Start()
    {
        localPlayer = Networking.LocalPlayer;
    }

    private void Update()
    {
        if(localPlayer != null)
        {
            transform.position = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        }
    }
}
