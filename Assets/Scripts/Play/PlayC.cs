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
    public PlayM GetPlayM { get { if (playM == null) { playM = gameObject.GetComponent<PlayM>(); } return playM; } }
    public PlayV GetPlayV { get { if (playV == null) { playV = gameObject.GetComponent<PlayV>(); } return playV; } }
    public List<MusicalInstrument> musicals;
    [HideInInspector]
    private int changeTone;
    private int needLoadCount;
    private int loadCompledCount;
    private bool Ready;
    private bool LoadCompled => needLoadCount == loadCompledCount;

    public void Start()
    {
        Init();
    }
    public void Update()
    {
        if (!Ready)
        {
            return;
        }

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

    private void Init()
    {
        var keys = new string[] { AddressablesLable.MusicalInstrument.ToString(),
        };
        needLoadCount = -1;
        loadCompledCount = 0;
        musicals = new List<MusicalInstrument>();
        //加载所有乐器
        AssetsLoad.Load<GameObject>(keys, OnLoadCompled, OnLoadCompleting);
    }

    /// <summary>
    /// 一个乐器实体加载完成时
    /// </summary>
    /// <param name="tempMusicla"></param>
    private void OnLoadCompleting(GameObject tempMusicla)
    {
        loadCompledCount++;
        Debug.Log($"加载{loadCompledCount}乐器");
        if (tempMusicla == null)
        {
            return;
        }
        else
        {
            var musical = GameObject.Instantiate(tempMusicla).GetComponent<MusicalInstrument>();
            this.musicals.Add(musical);
        }

        if (LoadCompled)
        {
            Debug.Log("单个加载所有完成");
            OnInitCompleting();
        }
    }

    /// <summary>
    /// 所有试题加载完成
    /// </summary>
    /// <param name="tempMusiclas"></param>
    private void OnLoadCompled(IList<GameObject> tempMusiclas)
    {
        Debug.Log("OnLoadCompled");

        if (tempMusiclas == null)
        {
            needLoadCount = 0;
        }
        else
        {
            needLoadCount = tempMusiclas.Count;
            tempMusiclas.Clear();
        }
        if (LoadCompled)
        {
            Debug.Log($"全部加载所有完成 {needLoadCount} 个");
            OnInitCompleting();
        }

    }

    /// <summary>
    /// 初始化完成中
    /// </summary>
    /// <param name="tempMusiclas"></param>
    private void OnInitCompleting()
    {
        Debug.Log("OnInitCompleting");

        GetPlayM.Init();

        List<string> options = new List<string>();
        for (int i = 0; i < musicals.Count; i++)
        {
            options.Add(musicals[i].musicalInstrumentName);
        }

        GetPlayV.InitComponent(options);
        GetPlayV.InitEvent(SetMusicalEvent, PlayToneEvent, StopToneEvent, ChageToneEvent);
    }

    /// <summary>
    /// 初始化已完成
    /// </summary>
    /// <param name="musical"></param>
    private void OnInitCompleted(MusicalInstrument musical)
    {
        GetPlayM.data.musicalInstrument = musical;
        //TODO 改变
        GetPlayV.ChangeToneEvent();

        GetPlayV.SetKeyTone(musical.KeyTone);

        Ready = true;
    }

    /// <summary>
    /// 设置乐器
    /// </summary>
    /// <param name="value"></param>
    public void SetMusicalEvent(int value)
    {
        MusicalInstrument musical = musicals[value];
        musical.Init(OnInitCompleted);
        Ready = false;
    }

    /// <summary>
    /// 获取乐器
    /// </summary>
    /// <returns></returns>
    public MusicalInstrument GetMusical()
    {
        return GetPlayM.data.musicalInstrument;
    }

    /// <summary>
    /// 播放乐器中的音效
    /// </summary>
    /// <param name="keyTone"></param>
    /// <param name="toneValue"></param>
    public void PlayToneEvent(string keyTone, int toneValue)
    {
        //某音源播放某音效
        toneValue = toneValue + changeTone;
        GetMusical().PlayTone(keyTone, toneValue);
    }
    public void StopToneEvent(string keyTone)
    {
        //某音源停止播放
        GetMusical().StopTone(keyTone);
    }

    /// <summary>
    /// 改变乐器时停止播放音效并重设对应UI颜色
    /// </summary>
    /// <param name="plays"></param>

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
        Debug.Log($"加载乐器 {musical.musicalInstrumentName}");
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