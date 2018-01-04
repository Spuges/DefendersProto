using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InputPoint
{
    public PlayerInputSender Component;
    public Vector2 Point;
    public int LastSign { get; set; }
}

// Late night inspiration, when pondering how to improve the super awkward controls
public class VectorInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public float Distance = 1f;
    public List<InputPoint> Inputs = new List<InputPoint>();

    bool hasFocus = false;
    private Vector2 point = Vector2.zero;
    
    private void Update()
    {
#if UNITY_EDITOR
        point = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        hasFocus = true;
#endif
        if (hasFocus)
        {


            onStay();
        }
    }

    private void onStay()
    {
        foreach(InputPoint inp in Inputs)
        {
            if (inp.Component == null)
                continue;

            float magn = Vector2.Scale(point, inp.Point).magnitude;
            float dot = Vector2.Dot(point, inp.Point);
            float delta = magn * dot;
            delta /= Distance;

            if (inp.Component && inp.Component.onInput != null)
            {
                inp.Component.onInput.Invoke(delta);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        updatePoint(eventData);
        hasFocus = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasFocus = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        updatePoint(eventData);
    }

    private void updatePoint(PointerEventData eData)
    {
        Vector3 pInWorld = eData.pointerCurrentRaycast.worldPosition - transform.position;
        point = new Vector2(pInWorld.x, pInWorld.y);
    }
}
