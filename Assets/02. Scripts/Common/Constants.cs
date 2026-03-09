using Unity.Android.Gradle.Manifest;
using UnityEngine;

public static class Constants
{
    public static readonly float Gravity = -9.81f;    

    public static LayerMask GroundLayerMask => LayerMask.GetMask("Ground");
}
