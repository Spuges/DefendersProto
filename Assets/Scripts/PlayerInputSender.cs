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
        OnPointerEnter(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerExit(eventData);
    }

    private IEnumerator inputUpdate()
    {
        while (hasFocus)
        {
            if (onInput != null)
                onInput.Invoke(InputValue);

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onInputDown != null)
            onInputDown.Invoke(InputValue);

        hasFocus = true;
        inpUpdate = StartCoroutine(inputUpdate());

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasFocus = false;

        if (inpUpdate != null)
            StopCoroutine(inpUpdate);

        if (onInputUp != null && hasFocus)
            onInputUp.Invoke(InputValue);
    }
}
