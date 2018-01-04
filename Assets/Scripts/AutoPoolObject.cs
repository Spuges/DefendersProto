using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPoolObject : MonoBehaviour, IReusable
{
    public int ObjectID
    {
        get
        {
            return objectID.Value;
        }

        set
        {
            objectID = value;
        }
    }

    private int? objectID;

    public bool Poolable { get { return objectID.HasValue; } }

    public GameObject GetObject { get { return gameObject; } }

    public void ResetObject()
    {
        this.enabled = true;
    }

    private void OnDisable()
    {
        ObjectPool.AddToReusePool(this);
    }
}
