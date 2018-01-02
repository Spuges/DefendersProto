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
    }

    public void StrafeDown()
    {

    }

    public void StrafeUp()
    {

    }

    // Strafing is actually up and down.
    public void Strafe(float velocity)
    {
        curStrafe = velocity;
    }

    public void Accelerate(float velocity)
    {
        curAccel = velocity;
    }

    private void LateUpdate()
    {
        if(Input.GetKey(KeyCode.DownArrow))
        {
            Strafe(-Strafing);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Strafe(Strafing);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Accelerate(-Acceleration);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Accelerate(Acceleration);
        }

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
        
        transform.Translate(transVel);

        curAccel = 0f;
        curStrafe = 0f;
    }
}
