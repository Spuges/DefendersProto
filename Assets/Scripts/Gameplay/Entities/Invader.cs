using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Entity
{
    protected override void Start()
    {
        base.Start();
        InvaderManager.RegisterNewInvader(this);

    }

    private void OnDisable()
    {
        InvaderManager.RemoveInvader(this);
    }
}
