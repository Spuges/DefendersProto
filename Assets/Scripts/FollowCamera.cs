using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class FollowCamera : MonoBehaviour
{
    public enum CameraPoint
    {
        LEFT = 1,
        RIGHT = 2
    }

    static FollowCamera _i;
    public Transform CameraTarget;

    public float LeftPointOffset;
    public float RightPointOffset;

    private float currentOffset = 0f;
    public float MaxDelta = 3f;

    public PID XController;
    
    public static void CameraTargetChanged(FollowCameraTarget target)
    {
        _i.CameraTarget = target.transform;
        _i.SwitchDirection((int)target.InitialDirection);
    }

    private void Awake()
    {
        _i = this;
    }

    // Only int, so i can message from unityevents..
    public void SwitchDirection(int dir)
    {
        CameraPoint point = (CameraPoint)dir;
        switch(point)
        {
            case CameraPoint.LEFT:
                currentOffset = LeftPointOffset;
                break;
            case CameraPoint.RIGHT:
                currentOffset = RightPointOffset;
                break;
            default:
                Debug.Log("Wut you do?");
                break;
        }
    }

    public void LateUpdate()
    {

        float x = CameraTarget.transform.position.x + XController.Update(CameraTarget.transform.position.x + currentOffset, transform.position.x, Time.deltaTime);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
