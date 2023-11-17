using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 变调
/// </summary>
public class ChangeTune : MonoBehaviour, IPointerClickHandler
{
    public int changeTuneValue { get; private set; } = 0;
    //预留1个高音半音 用于升调
    private const int minTune = -4 * 12;
    private const int maxTune = 12 - 1 - 1;
    private Dropdown dropdown;
    private List<string> options;
    private UnityAction ChangeToneEvent;
    public void Init(UnityAction ChangeToneEvent)
    {
        this.ChangeToneEvent = ChangeToneEvent;
        dropdown = gameObject.GetComponent<Dropdown>();
        options = new List<string>();
        for (int i = minTune; i <= maxTune; i++)
        {
            options.Add(i.ToString());
        }

        dropdown.onValueChanged.AddListener(RisingTune);

        dropdown.AddOptions(options);
        //dropdown.value = -1;
    }
    //"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"
    //最多可以升多少呢 键盘是C3 到 C7
    //音源是 [C-1]到B7
    //最多可以升 B7-C7-1 = 11 个半音
    //最多可以降 C3-[C-1] = 4 * 12  =  个半音
    public void RisingTune(int value)
    {
        changeTuneValue = int.Parse(options[value]);
        ChangeToneEvent.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scrollbar scrollbar = dropdown.transform.Find("Dropdown List").GetComponentInChildren<Scrollbar>();
        float scale = 1.0f / dropdown.options.Count;
        float value = 1 - dropdown.value * scale;
        if (value < 0.5f)
        {
            value = value - scale;
        }
        scrollbar.value = value;
    }

    public void SetKeyTone(int keyTone)
    {
        int length = options.Count;
        string key = keyTone.ToString();
        for (int i = 0; i < length; i++)
        {
            if (key == options[i])
            {
                dropdown.value = i;
                break;
            }
        }
    }
}
