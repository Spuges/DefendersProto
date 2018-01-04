using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

/// <summary>
/// Base class for gameplay entities
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Entity : MonoBehaviour, IReusable
{
    // Set after instantiaton, from the original object.
    // Only used for object pooling and comparison of types..
    public int ObjectID 
    {
        get
        {
            if (objectID.HasValue)
                return objectID.Value;
            else
                return GetInstanceID();
        }
        set
        {
            objectID = value;
        }
    }

    public bool Poolable
    {
        get { return objectID.HasValue; }
    }

    private int? objectID = null;

    public int Health = 1;
    public int GetCurrentHealth { get { return currentHealth; } }
    private int currentHealth;

    // Triggering purposes
    protected Rigidbody2D rigid;

    public UnityEvent OnObjectReset;
    public UnityEvent OnDamageTaken;
    public UnityEvent OnDestroyed;
    
    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        ResetObject();
    }

    // Object reseting.. Am I really pooling these? I Guess I am.
    public virtual void ResetObject()
    {
        currentHealth = Health;

        gameObject.SetActive(true);

        if(OnObjectReset != null)
            OnObjectReset.Invoke();
    }

    public GameObject GetObject
    {
        get { return gameObject; }
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

    void OnParticleCollision(GameObject other)
    {
        DamageSource source = null;
        if (source = other.GetComponent<DamageSource>())
        {
            // Debug.Log("Damage!!; " + source.Damage);
            DamageTaken(source.Damage);
            source.DamageDone();
        }
    }

    public void DisableAndReuse()
    {
        gameObject.SetActive(false);
        ObjectPool.AddToReusePool(this);
    }

    public void InstantiateObject(GameObject obj)
    {
        Instantiate(obj, transform.position, Quaternion.identity);
    }
}
