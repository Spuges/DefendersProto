using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class Defender : Entity
{
    public float Acceleration = 1f;
    public float Strafing = 0.5f;

    public GameObject ProjectilePrefab;

    public Vector2 HeightBounds;

    // Guess I'll just put the player inputs here, uh?
    private float curVel;     // Velocity
    public float MaxAcceleration;
    private float curAccel;     // Acceleration
    public float MaxStrafe;
    private float curStrafe;    // Strafe

    public override void ResetObject()
    {
        base.ResetObject();

        curVel = 0f;
        curAccel = 0f;
        curStrafe = 0f;

        registerInputs();
    }

    public override void Destroyed()
    {
        base.Destroyed();
        PlayerInputSender.UnRegisterInputs();
    }

    // Strafing is actually up and down.
    public void Strafe(float val)
    {
        curStrafe = Strafing * val;
    }

    public void StrafeDown(float val)
    {
    }

    public void StrafeUp(float val)
    {
    }

    public void Accelerate(float val)
    {
        curAccel = Acceleration * val;
    }
    
    private void LateUpdate()
    {
        if (!Mathf.Approximately(0f, curAccel))
        {
            curVel += curAccel * Time.deltaTime;
            curVel = Mathf.Clamp(curVel, -MaxAcceleration, MaxAcceleration);
        }
        else
            curVel = Mathf.MoveTowards(curVel, 0, Acceleration * Time.deltaTime / 2f);

        curStrafe = curStrafe * Time.deltaTime;
        curStrafe = Mathf.Clamp(curStrafe, HeightBounds.x - transform.position.y, HeightBounds.y - transform.position.y);

        Vector2 transVel = new Vector2(curVel, curStrafe);

        transform.position += new Vector3(transVel.x, transVel.y, 0f);

        curAccel = 0f;
        curStrafe = 0f;
    }

    private void registerInputs()
    {
        PlayerInputSender.RegisterInputAction(PInput.LEFT, PInputType.FOCUS, Accelerate);
        PlayerInputSender.RegisterInputAction(PInput.RIGHT, PInputType.FOCUS, Accelerate);

        PlayerInputSender.RegisterInputAction(PInput.UP, PInputType.FOCUS, Strafe);
        PlayerInputSender.RegisterInputAction(PInput.DOWN, PInputType.FOCUS, Strafe);
    }
}
