using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrientation
{
    FollowCamera.TargetOrientation Orientation { get; set; }
}