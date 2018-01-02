using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class Defender : Entity
{
    public float Acceleration = 1f;
    public float Strafing = 0.5f;

    public GameObject ProjectilePrefab;

    // Guess I'll just put the player inputs here, uh?
    public PIDV2 PIDCtrl;       // Falloff
    private Vector2 curVel;     // Velocity
    public float MaxAcceleration;
    private float curAccel;     // Acceleration
    public float MaxStrafe;
    private float curStrafe;    // Strafe

    public override void ResetObject()
    {
        base.ResetObject();

        curVel = Vector3.zero;
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
        curStrafe += velocity * Time.deltaTime;
        curStrafe = Mathf.Clamp(curStrafe, -MaxStrafe, MaxStrafe);
    }

    public void Accelerate(float velocity)
    {
        curAccel += velocity * Time.deltaTime;
        curAccel = Mathf.Clamp(curAccel, -MaxAcceleration, MaxAcceleration);
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

        Vector2 newVel = new Vector2(curVel.x + curAccel, curVel.y + curStrafe);

        transform.Translate(PIDCtrl.Update(newVel, transform.position, Time.deltaTime));
    }
}
