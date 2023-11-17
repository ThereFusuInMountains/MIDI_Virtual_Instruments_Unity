using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayFingerShake : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [HideInInspector]
    public bool IsShake = false;
    private Color old;
    private Image image;
    public KeyCode keyCode;
    public void Awake()
    {
        image = gameObject.GetComponent<Image>();
        old = image.color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartShake();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EndShake();
    }

    public void StartShake()
    {
        IsShake = true;
        image.color = old / 2;
    }
    public void EndShake()
    {
        IsShake = false;
        image.color = old;
    }
}
