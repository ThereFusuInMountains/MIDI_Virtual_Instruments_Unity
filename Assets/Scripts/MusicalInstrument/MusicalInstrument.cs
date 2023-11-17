using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalInstrument : MonoBehaviour
{
    private Dictionary<string, AudioSource> dictSource;
    //key:音效AddressablesKey value：toneValue
    private Dictionary<string, int> dictClipTone;
    [SerializeField]
    private AudioClip[] audioClip;
    [SerializeField]
    private bool upStop = true;
    public string musicalInstrumentName;
    [SerializeField]
    private int keyTone;
    [SerializeField]
    private AudioFormat audioFormat;

    /// <summary>
    /// 已经准备好，可以使用了
    /// </summary>
    private bool Ready;

    private Action<MusicalInstrument> OnReadyCompleted;
    /// <summary>
    /// 基调/主调
    /// </summary>
    public int KeyTone { get { return keyTone; } }

    public void Init(Action<MusicalInstrument> onInitCompleted)
    {
        if (Ready)
        {
            onInitCompleted?.Invoke(this);
            return;
        }

        OnReadyCompleted = onInitCompleted;

        LoadClip(musicalInstrumentName);

        int length = transform.childCount;

        dictSource = new Dictionary<string, AudioSource>();

        for (int i = 0; i < length; i++)
        {
            Transform child = transform.GetChild(i);
            dictSource.Add(child.name, child.GetComponent<AudioSource>());
        }
    }

    public void PlayTone(string keyTone, int toneValue)
    {
        dictSource[keyTone].PlayOneShot(audioClip[toneValue]);
    }

    public void StopTone(string keyTone)
    {
        if (upStop)
        {
            dictSource[keyTone].Stop();
        }
    }

    public bool ExistClip(int toneValue)
    {
        if (toneValue >= audioClip.Length)
        {
            return false;
        }
        if (toneValue < 0)
        {
            return false;
        }
        if (audioClip[toneValue] == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 获取音符的key
    /// </summary>
    /// <param name="musicalName">乐器名</param>
    /// <param name="format">音频格式</param>
    /// <param name="step">音阶</param>
    /// <param name="degree">音程</param>
    /// <returns></returns>
    private string GetToneKey(string musicalName, string format, int step, int degree)
    {
        return $"{musicalInstrumentName}_{Tone.GetToneName()[step]}{degree}";
    }

    private void LoadClip(string musicalInstrumentName)
    {
        string format = Enum.GetName(audioFormat.GetType(), audioFormat);
        /**
         * 乐器的音阶范围为 
         * "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" 
         * -1 0 1 2 3 4 5 6 7 
         * 组合
         **/

        dictClipTone = new Dictionary<string, int>();
        var needToneCount = 0;
        var length = Tone.GetToneName().Length;

        for (int degree = -1; degree <= 7; degree++)
        {
            for (int step = 0; step < length; step++)
            {
                var key = GetToneKey(musicalInstrumentName, format, step, degree);
                dictClipTone.Add(key, needToneCount);
                needToneCount++;
            }
        }
        audioClip = new AudioClip[needToneCount];

        LoadArray<AudioClip>(musicalInstrumentName, LoadCompleted, LoadCompleting);
    }

    private void LoadArray<T>(string label, Action<IList<T>> completed, Action<T> completing) where T : UnityEngine.Object
    {
        Debug.Log(label);
        AssetsLoad.Load<T>(new string[] { label }, completed, completing);
    }

    private void LoadCompleted(IList<AudioClip> clips)
    {
        this.Ready = true;
        OnReadyCompleted?.Invoke(this);
    }

    private void LoadCompleting(AudioClip audio)
    {
        var key = audio.name;
        var index = dictClipTone[key];
        audioClip[index] = audio;
    }

    private enum AudioFormat
    {
        OGG,
        WAV,
    }
}