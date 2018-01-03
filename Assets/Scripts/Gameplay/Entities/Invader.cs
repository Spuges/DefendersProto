using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Entity
{
    public DamageSource CollisionDamageSource;

    protected override void Start()
    {
        base.Start();
        InvaderManager.RegisterNewInvader(this);

        if(CollisionDamageSource == null)
            CollisionDamageSource = GetComponent<DamageSource>();
    }

    private void OnDisable()
    {
        InvaderManager.RemoveInvader(this);
    }

    private void Update()
    {
        if(!InvaderManager.IsInsideBounds(new Vector2(transform.position.x, transform.position.y)))
        {

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
