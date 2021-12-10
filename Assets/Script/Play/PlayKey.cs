using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayKey : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [HideInInspector]
    public int toneValue;
    [HideInInspector]
    public string keyTone;
    public KeyCode keyCode;
    private UnityAction<string, int> PlayToneEvent;
    private UnityAction<string> StopToneEvent;
    private Color oldColor;
    private Color normalColor;
    private Color disableColro = Color.grey;
    private Image image;
    public bool IsUse { get; private set; }

    public void Init(UnityAction<string,int> PlayToneEvent, UnityAction<string> StopToneEvent)
    {
        keyTone = name;
        toneValue = Tone.dict[keyTone];
        this.PlayToneEvent = PlayToneEvent;
        this.StopToneEvent = StopToneEvent;
        image = gameObject.GetComponent<Image>();
        oldColor= normalColor = image.color;
        IsUse = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        KeyDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        KeyUp();
    }

    public void KeyDown()
    {
        PlayToneEvent.Invoke(keyTone, toneValue);
        oldColor = image.color;
        image.color = image.color / 2;
        IsUse = true;
    }

    public void KeyUp()
    {
        StopToneEvent.Invoke(keyTone);
        image.color = oldColor;
        IsUse = false;
    }

    public void Show()
    {
        image.color = normalColor;
    }

    public void Hide()
    {
        image.color = disableColro;
    }
}
