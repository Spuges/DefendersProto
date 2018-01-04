using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class FollowCameraTarget : MonoBehaviour
{
    public FollowCamera.TargetOrientation InitialDirection = FollowCamera.TargetOrientation.LEFT;
    private FollowCamera.TargetOrientation currentOrientation = FollowCamera.TargetOrientation.LEFT;
    public FloatEvent OnDirectionChanged; // Acceleration direction changes

    private void Start()
    {
        currentOrientation = InitialDirection;
        FollowCamera.CameraTargetChanged(this);

        PlayerInputSender.RegisterInputAction(PInput.HORIZONTAL, PInputType.FOCUS, onHorizontalMovement);
    }

    public void ChangeDirection(float dir)
    {
        if(dir < 0f)
            FollowCamera.ChangeDirection(FollowCamera.TargetOrientation.LEFT);
        else
            FollowCamera.ChangeDirection(FollowCamera.TargetOrientation.RIGHT);
    }

    private void onHorizontalMovement(float val)
    {
        if (val < 0f && currentOrientation == FollowCamera.TargetOrientation.RIGHT)
        {
            currentOrientation = FollowCamera.TargetOrientation.LEFT;
            FollowCamera.ChangeDirection(currentOrientation);

            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
            if (OnDirectionChanged != null)
                OnDirectionChanged.Invoke(val);
        }
        else if(0f < val && currentOrientation == FollowCamera.TargetOrientation.LEFT)
        {
            currentOrientation = FollowCamera.TargetOrientation.RIGHT;
            FollowCamera.ChangeDirection(currentOrientation);

            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

            if (OnDirectionChanged != null)
                OnDirectionChanged.Invoke(val);
        }
    }
}
