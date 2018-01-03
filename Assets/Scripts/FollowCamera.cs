using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class FollowCamera : MonoBehaviour
{
    public enum TargetOrientation
    {
        LEFT = 1,
        RIGHT = 2
    }

    static FollowCamera _i;
    public Transform CameraTarget;

    public float LeftPointOffset;
    public float RightPointOffset;

    private float targetOffset = 0f;
    private float currentOffset = 0f;
    public float MaxDelta = 3f;

    public PID XController;
    
    public static void CameraTargetChanged(FollowCameraTarget target)
    {
        _i.CameraTarget = target.transform;
        _i.switchDirection(target.InitialDirection);
    }

    public static void ChangeDirection(TargetOrientation dir)
    {
        _i.switchDirection(dir);
    }

    private void Awake()
    {
        _i = this;
    }
    
    private void switchDirection(TargetOrientation dir)
    {
        switch(dir)
        {
            case TargetOrientation.LEFT:
                targetOffset = LeftPointOffset;
                break;
            case TargetOrientation.RIGHT:
                targetOffset = RightPointOffset;
                break;
            default:
                break;
        }
    }

    public void LateUpdate()
    {
        currentOffset += XController.Update(targetOffset, currentOffset, Time.deltaTime);
        float x = CameraTarget.position.x + currentOffset;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
