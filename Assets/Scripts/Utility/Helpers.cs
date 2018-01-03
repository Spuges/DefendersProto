using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Helpers
{
    public class FloatEvent : UnityEvent<float> { }

    public static class Util
    {

    }

    /// <summary>
    /// Basic PID controller
    /// </summary>
    [System.Serializable]
    public class PID
    {
        public float pFactor, iFactor, dFactor;

        float integral;
        float lastError;

        public PID(float pFactor, float iFactor, float dFactor)
        {
            this.pFactor = pFactor;
            this.iFactor = iFactor;
            this.dFactor = dFactor;
        }

        public float Update(float setpoint, float actual, float timeFrame)
        {
            float present = setpoint - actual;
            integral += present * timeFrame;
            float deriv = (present - lastError) / timeFrame;
            lastError = present;
            return present * pFactor + integral * iFactor + deriv * dFactor;
        }
    }

    /// <summary>
    /// Basic Vector2 PID controller
    /// </summary>
    [System.Serializable]
    public class PIDV2
    {
        public float pFactor, iFactor, dFactor;

        Vector2 integral;
        Vector2 lastError;

        public PIDV2(float pFactor, float iFactor, float dFactor)
        {
            this.pFactor = pFactor;
            this.iFactor = iFactor;
            this.dFactor = dFactor;
        }

        public Vector2 Update(Vector2 setpoint, Vector2 actual, float timeFrame)
        {
            Vector2 present = setpoint - actual;
            integral += present * timeFrame;
            Vector2 deriv = (present - lastError) / timeFrame;
            lastError = present;
            return present * pFactor + integral * iFactor + deriv * dFactor;
        }
    }

    /// <summary>
    /// Basic Vector3 PID controller
    /// </summary>
    [System.Serializable]
    public class PIDV3
    {
        public float pFactor, iFactor, dFactor;

        Vector3 integral;
        Vector3 lastError;

        public PIDV3(float pFactor, float iFactor, float dFactor)
        {
            this.pFactor = pFactor;
            this.iFactor = iFactor;
            this.dFactor = dFactor;
        }

        public Vector3 Update(Vector3 setpoint, Vector3 actual, float timeFrame)
        {
            Vector3 present = setpoint - actual;
            integral += present * timeFrame;
            Vector3 deriv = (present - lastError) / timeFrame;
            lastError = present;
            return present * pFactor + integral * iFactor + deriv * dFactor;
        }
    }
}
