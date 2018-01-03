using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<int, Queue<IReusable>> reusePool;

    public static ObjectPool I
    {
        get
        {
            if (_i == null)
                _i = FindObjectOfType<ObjectPool>();

            return _i;
        }
        set
        {
            if (_i == null)
                _i = FindObjectOfType<ObjectPool>();

            _i = value;
        }
    }
    private static ObjectPool _i;
    
    public static void AddToReusePool(IReusable obj)
    {
        if (!obj.Poolable)
            Destroy(obj.GetObject); // Unpoolable, let GC have this one.

        if (I.reusePool == null)
            I.reusePool = new Dictionary<int, Queue<IReusable>>();

        if (!I.reusePool.ContainsKey(obj.ObjectID))
            I.reusePool.Add(obj.ObjectID, new Queue<IReusable>());

        I.reusePool[obj.ObjectID].Enqueue(obj);
    }

    public static T GetObjectFromPool<T>(IReusable obj) where T : Object
    {
        if (!I.reusePool.ContainsKey(obj.ObjectID) || I.reusePool[obj.ObjectID].Count == 0)
            return null;

        return (T)I.reusePool[obj.ObjectID].Dequeue();
    }

    public static T CreateObject<T>(T original) where T : Object
    {
        IReusable res = (IReusable)original;
        T newObj = null;

        if(res != null)
        {
            newObj = GetObjectFromPool<T>(res);
        }

        if(!newObj)
        {
            newObj = Instantiate(original);
            res = (IReusable)newObj;
            if (res != null)
                res.ObjectID = original.GetInstanceID();
        }

        return newObj;
    }
}
