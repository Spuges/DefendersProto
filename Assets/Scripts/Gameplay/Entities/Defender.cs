using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class Defender : Entity
{
    public float Acceleration = 1f;
    public float Strafing = 0.5f;

    public GameObject ProjectilePrefab;

    public Vector2 HeightBounds { get; set; }

    // Guess I'll just put the player inputs here, uh?
    private float curVel;     // Velocity
    public float MaxAcceleration;
    private float curAccel;     // Acceleration
    private float curStrafe;    // Strafe

    protected override void Start()
    {
        HeightBounds = new Vector2(-InvaderManager.I.InvaderBounds.y, InvaderManager.I.InvaderBounds.y);
        base.Start();

        registerInputs();
    }

    public override void ResetObject()
    {
        base.ResetObject();

        curVel = 0f;
        curAccel = 0f;
        curStrafe = 0f;
    }

    public override void Destroyed()
    {
        base.Destroyed();
        gameObject.SetActive(false);
        InvaderManager.I.SpawnPlayer();
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
        PlayerInputSender.RegisterInputAction(PInput.HORIZONTAL, PInputType.FOCUS, Accelerate);
        PlayerInputSender.RegisterInputAction(PInput.VERTICAL, PInputType.FOCUS, Strafe);
    }
}
