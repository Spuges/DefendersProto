using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PInput
{
    HORIZONTAL,
    VERTICAL,
    FIRE
}

public enum PInputType
{
    GAINED_FOCUS,
    FOCUS,
    LOST_FOCUS,
}

public class PlayerInputSender : MonoBehaviour
{
    public static List<PlayerInputSender> senders;

    public System.Action<float> onInputDown { get; set; }
    public System.Action<float> onInputUp { get; set; }
    public System.Action<float> onInput { get; set; }

    public PInput InputType;
    public float InputValue = 0f;

    public static void UnRegisterInputs()
    {
        foreach(PlayerInputSender pise in senders)
        {
            pise.clear();
        }
    }

    public static void RegisterInputAction(PInput input, PInputType type, System.Action<float> action)
    {
        List<PlayerInputSender> inputListeners = senders.FindAll(o => o.InputType == input);
        if(inputListeners.Count > 0)
        {
            foreach(PlayerInputSender pis in inputListeners)
                pis.setup(type, action);
        }
        else
        {
            Debug.Log("No input senders of such type: " + input.ToString());
        }
    }

    private void Awake()
    {
        if (senders == null)
            senders = new List<PlayerInputSender>();

        senders.Add(this);
    }

    protected void setup(PInputType type, System.Action<float> action)
    {
        switch(type)
        {
            case PInputType.FOCUS:
                onInput += action;
                break;
            case PInputType.GAINED_FOCUS:
                onInputDown += action;
                break;
            case PInputType.LOST_FOCUS:
                onInputUp += action;
                break;
        }
    }

    protected void clear()
    {
        onInputDown = null;
        onInputUp = null;
        onInput = null;
    }
}
