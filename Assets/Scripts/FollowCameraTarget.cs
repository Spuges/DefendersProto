using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraTarget : MonoBehaviour
{
    public FollowCamera.CameraPoint InitialDirection = FollowCamera.CameraPoint.LEFT;

    private void Start()
    {
        FollowCamera.CameraTargetChanged(this);
    }
}
