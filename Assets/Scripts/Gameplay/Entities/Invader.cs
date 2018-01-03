using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Entity
{
    private void OnEnable()
    {
        InvaderManager.RegisterNewInvader(this);
    }

    private void OnDisable()
    {
        InvaderManager.RemoveInvader(this);
    }
}
