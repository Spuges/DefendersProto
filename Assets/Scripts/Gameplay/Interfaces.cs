using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrientation
{
    FollowCamera.TargetOrientation Orientation { get; set; }
}

public interface IReusable
{
    int ObjectID { get; set; }
    bool Poolable { get; }
    void ResetObject();
    GameObject GetObject { get; }
}