using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// 升调
/// </summary>
public class RisingTune : MonoBehaviour,IPointerUpHandler,IPointerDownHandler
{
    public KeyCode keyCode;
    public int tuneValue { get; private set; } = 0;
    private Image image;
    private Color oldColor;
    private UnityAction ChangeToneEvent;
    public void Init(UnityAction ChangeToneEvent)
    {
        this.ChangeToneEvent = ChangeToneEvent;
        image = gameObject.GetComponent<Image>();

        oldColor = image.color;
    }
    public void OnDown()
    {
        tuneValue = 1;
        image.color = image.color / 2;
        ChangeToneEvent.Invoke();
    }

    public void OnUp()
    {
        tuneValue = 0;
        image.color = oldColor;
        ChangeToneEvent.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp();
    }
}
