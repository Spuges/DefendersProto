using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Entity
{
    public DamageSource CollisionDamageSource;

    protected override void Start()
    {
        base.Start();

        if(CollisionDamageSource == null)
            CollisionDamageSource = GetComponent<DamageSource>();
    }

    public override void ResetObject()
    {
        base.ResetObject();
        InvaderManager.RegisterNewInvader(this);
    }

    private void OnDisable()
    {
        InvaderManager.RemoveInvader(this);
    }

    private void Update()
    {
        if(!InvaderManager.IsInsideBounds(new Vector2(transform.position.x, transform.position.y)))
        {
            DisableAndReuse();
            InvaderManager.I.SpawnInvader(); // Just spawn a new invader
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Defender dfndr = null;
        if(dfndr = collision.GetComponent<Defender>())
        {
            if(CollisionDamageSource)
            {
                dfndr.DamageTaken(CollisionDamageSource.Damage);

                if (CollisionDamageSource.OnDamageDone != null)
                    CollisionDamageSource.OnDamageDone.Invoke();
            }
        }
    }
}
