using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageSource : MonoBehaviour
{
    public UnityEvent OnDamageDone;
    public int Damage = 1;

    public void DamageDone()
    {
        if(OnDamageDone != null)
        {
            OnDamageDone.Invoke();
        }
    }
}
