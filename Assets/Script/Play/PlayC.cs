using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayM))]
[RequireComponent(typeof(PlayV))]
public class PlayC : MonoBehaviour
{
    private PlayM playM;
    private PlayV playV;
    public PlayM GetPlayM{ get{if (playM == null){ playM = gameObject.GetComponent<PlayM>(); } return playM;  } }
    public PlayV GetPlayV { get { if (playV == null) { playV = gameObject.GetComponent<PlayV>(); }return playV; } }
    public MusicalInstrument[] musicals;
    [HideInInspector]
    private int changeTone;
    public void Start()
    {
        Init();
    }
    public void Update()
    {
        #region 变调
        if (Input.GetKeyDown(GetPlayV.risingTune.keyCode))
        {
            GetPlayV.risingTune.OnDown();
        }
        if (Input.GetKeyUp(GetPlayV.risingTune.keyCode))
        {
            GetPlayV.risingTune.OnUp();
        }
        #endregion

        int length = GetPlayV.playKey.Count;

        for (int i = 0; i < length; i++)
        {
            if (Input.GetKeyDown(GetPlayV.playKey[i].keyCode))
            {
                GetPlayV.playKey[i].KeyDown();
            }
            if (Input.GetKeyUp(GetPlayV.playKey[i].keyCode))
            {
                GetPlayV.playKey[i].KeyUp();
            }
        }
    }
    public void Init()
    {
        if (musicals.Length == 0)
        {
            Debug.LogError("目前没有乐器");
        }
        //for (int i = 0; i < musicals.Length; i++)
        //{
        //    musicals[i].Init();
        //}
        GetPlayM.Init();

        List<string> options = new List<string>();
        for (int i = 0; i < musicals.Length; i++)
        {
            options.Add(musicals[i].musicalInstrumentName);
        }

        GetPlayV.InitComponent(options);
        GetPlayV.InitEvent(SetMusicalEvent, PlayToneEvent, StopToneEvent, ChageToneEvent);
    }
    
    public void SetMusicalEvent(int value)
    {
        MusicalInstrument musical = musicals[value];
        musical.Init();
        GetPlayM.data.musicalInstrument = musical;
        
        GetPlayV.ChangeToneEvent();

        GetPlayV.SetKeyTone(musical.KeyTone);
    }
    public MusicalInstrument GetMusical()
    {
        return GetPlayM.data.musicalInstrument;
    }
    public void PlayToneEvent(string keyTone, int toneValue)
    {
        //某音源播放某音效
        toneValue = toneValue + changeTone;
        GetMusical().PlayTone(keyTone,toneValue);
    }
    public void StopToneEvent(string keyTone)
    {
        //某音源停止播放
        GetMusical().StopTone(keyTone);
    }

    public void ChageToneEvent(List<PlayKey> plays)
    {
        //1、变调时应该停止所有按下键的音源播放。并且对其重新播放音源，所以需要一个标记[use]确定是否按下但是没有弹起
        //2、变调时应该更改是否有音源 如果没有应该改为灰色，若果有应该改为 正常颜色
        changeTone = GetPlayV.risingTune.tuneValue + GetPlayV.changeTune.changeTuneValue;
        MusicalInstrument musical = GetMusical();
        if (musical == null)
        {
            Debug.Log("musical is null");
            return;
        }
        Debug.Log(musical.musicalInstrumentName);
        int length = plays.Count;
        for (int i = 0; i < length; i++)
        {
            if (plays[i].IsUse)
            {
                StopToneEvent(plays[i].keyTone);
                PlayToneEvent(plays[i].keyTone, plays[i].toneValue);
            }

            if (musical.ExistClip(plays[i].toneValue + changeTone))
            {
                plays[i].Show();
            }
            else
            {
                plays[i].Hide();
            }
        }
    }
}