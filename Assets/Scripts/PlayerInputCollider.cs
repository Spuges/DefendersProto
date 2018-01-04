using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputCollider : PlayerInputSender, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool hasFocus = false;

    private Coroutine inpUpdate = null;
    
    private void Awake()
    {
        if (senders == null)
            senders = new List<PlayerInputSender>();

        senders.Add(this);
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
