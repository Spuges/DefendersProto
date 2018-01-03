using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

public class PlayerInputSender : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static List<PlayerInputSender> senders;

    public System.Action<float> onInputDown { get; set; }
    public System.Action<float> onInputUp { get; set; }
    public System.Action<float> onInput { get; set; }

    public PInput InputType;
    public float InputValue = 0f;

    private bool wasClicked = false;
    private bool hasFocus = false;

    private Coroutine inpUpdate = null;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        wasClicked = true;

        if (inpUpdate != null)
            StopCoroutine(inpUpdate);

        inpUpdate = StartCoroutine(inputUpdate());

        if (onInputDown != null)
            onInputDown.Invoke(InputValue);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (wasClicked)
        {
            if (onInputUp != null)
                onInputUp.Invoke(InputValue);

            wasClicked = false;
        }
    }

    private IEnumerator inputUpdate()
    {
        while (wasClicked)
        {
            if (hasFocus && onInput != null)
                onInput.Invoke(InputValue);

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hasFocus = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasFocus = false;
    }
}
