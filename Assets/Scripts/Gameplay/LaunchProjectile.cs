using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LaunchProjectile : MonoBehaviour
{
    public UnityEvent OnFire;
    public bool RegisterInputFire = false;

    public float CoolDown = 1f;
    private float lastLaunch = 0f;

    private void Start()
    {
        if(RegisterInputFire)
        {
            PlayerInputSender.RegisterInputAction(PInput.FIRE, PInputType.FOCUS, TryToFire);
        }
    }

    public void TryToFire(float val)
    {
        if(gameObject.activeInHierarchy && lastLaunch < Time.time)
        {
            lastLaunch = Time.time + CoolDown;

            if (OnFire != null)
                OnFire.Invoke();
        }
    }

}
