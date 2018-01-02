using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PInput
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
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

    private bool wasClicked = false;

    public static void UnRegisterInputs()
    {
        foreach(PlayerInputSender pise in senders)
        {
            pise.clear();
        }
    }

    public static void RegisterInputAction(PInput input, PInputType type, System.Action<float> action)
    {
        PlayerInputSender pinput = senders.Find(o => o.InputType == input);
        if(pinput)
        {
            pinput.setup(type, action);
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

    public void OnMouseDown()
    {
        wasClicked = true;
        if (onInputDown != null)
            onInputDown.Invoke(InputValue);
    }

    public void OnMouseOver()
    {
        if(wasClicked)
        {
            if (onInput != null)
                onInput.Invoke(InputValue);
        }
    }

    public void OnMouseUp()
    {
        if (wasClicked)
        {
            if (onInputUp != null)
                onInputUp.Invoke(InputValue);
        }
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
