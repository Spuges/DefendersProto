using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Base class for gameplay entities
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour
{
    public int Health = 1;
    private int currentHealth;

    // Triggering purposes
    protected Rigidbody2D rigid;

    public UnityEvent OnObjectReset;
    public UnityEvent OnDamageTaken;
    public UnityEvent OnDestroyed;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        ResetObject();
    }

    // Object reseting.. Am I really pooling these? I Guess I am.
    public virtual void ResetObject()
    {
        currentHealth = Health;

        if(OnObjectReset != null)
            OnObjectReset.Invoke();
    }

    public virtual void DamageTaken(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Destroyed();
        }
        else if (OnDamageTaken != null)
        {
            OnDamageTaken.Invoke();
        }
    }

    public virtual void Destroyed()
    {
        if (OnDestroyed != null)
            OnDestroyed.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision; " + collision.name);
        DamageSource source = null;
        if (source = collision.GetComponent<DamageSource>())
        {
            Debug.Log("Damage!!; " + source.Damage);
            DamageTaken(source.Damage);
            source.DamageDone();
        }
    }
}
