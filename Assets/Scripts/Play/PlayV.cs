using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayV : MonoBehaviour
{
    #region top
    private RectTransform top;
    /// <summary>
    /// 简谱
    /// </summary>
    private Toggle numberedMusicalNotation;
    /// <summary>
    /// 工尺谱
    /// </summary>
    private Toggle chineseMusic;
    private Toggle midi;
    /// <summary>
    /// 练习 自己听
    /// </summary>
    private Toggle study;
    /// <summary>
    /// 演奏 所有人听
    /// </summary>
    private Toggle Play;
    /// <summary>
    /// 乐器模式/那件乐器
    /// </summary>
    private Dropdown model;
    #endregion
    #region key
    private RectTransform key;
    public List<PlayKey> playKey;
    #endregion
    #region Right
    private RectTransform right;
    private PlayFingerShake fingerShake;
    [HideInInspector]
    public RisingTune risingTune;
    [HideInInspector]
    public ChangeTune changeTune;
    #endregion
    private UnityAction<List<PlayKey>> changeToneEvent;
    public void InitComponent(List<string> options)
    {
        #region top
        top = transform.Find("Top") as RectTransform;

        numberedMusicalNotation = top.Find("Notation/NumberedMusicalNotation").GetComponent<Toggle>();
        chineseMusic = top.Find("Notation/ChineseMusic").GetComponent<Toggle>();

        midi = top.Find("MIDI").GetComponent<Toggle>();

        //计划
        study = top.Find("Schema/Study").GetComponent<Toggle>();
        Play = top.Find("Schema/Play").GetComponent<Toggle>();

        model = top.Find("Model").GetComponent<Dropdown>();

        model.AddOptions(options);
        #endregion

        #region key
        key = transform.Find("key") as RectTransform;

        playKey = new List<PlayKey>();
        #endregion

        #region right
        right = transform.Find("Right") as RectTransform;
        fingerShake = right.Find("FingerShake").GetComponent<PlayFingerShake>();
        risingTune = right.Find("RisingTune").GetComponent<RisingTune>();

        changeTune = right.Find("ChangeTune").GetComponent<ChangeTune>();
        #endregion
    }

    public void InitEvent(UnityAction<int> SetMusicalInstrumentEvent, UnityAction<string, int> PlayToneEvent, UnityAction<string> StopToneEvent, UnityAction<List<PlayKey>> ChangeToneEvent)
    {
        #region Event
        this.changeToneEvent = ChangeToneEvent;

        int length = key.childCount;

        for (int i = 0; i < length; i++)
        {
            PlayKey play = key.GetChild(i).GetComponent<PlayKey>();
            play.Init(PlayToneEvent, StopToneEvent);
            playKey.Add(play);
        }

        risingTune.Init(this.ChangeToneEvent);

        changeTune.Init(this.ChangeToneEvent);

        model.onValueChanged.AddListener(SetMusicalInstrumentEvent);
        model.value = -1;
        #endregion
    }

    public bool SetMIDI
    {
        set { midi.isOn = value; }
    }
    public bool GetMIDI
    {
        get { return midi.isOn; }
    }

    public Notation SetNotation
    {
        set
        {
            if (value == Notation.ChineseMusic) { chineseMusic.isOn = true; }
            else { numberedMusicalNotation.isOn = true; }
        }
    }
    public Notation GetNotation
    {
        get
        {
            if (chineseMusic.isOn)
            {
                return Notation.ChineseMusic;
            }
            else
            {
                return Notation.NumberedMusicalNotation;
            }
        }
    }

    public Schema SetSchema
    {
       set
        {
            if (Schema.Play == value)
            {
                Play.isOn = true;
            }
            else
            {
                study.isOn = true;
            }
        }
    }
    public Schema GetSchema
    {
        get
        {
            if (Play.isOn)
            {
                return Schema.Play;
            }
            else
            {
                return Schema.Study;
            }
        }
    }

    public void ChangeToneEvent()
    {
        changeToneEvent.Invoke(playKey);
    }

    public void SetKeyTone(int keyTone)
    {
        changeTune.SetKeyTone(keyTone);
    }
}
