using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalInstrument : MonoBehaviour
{
    private Dictionary<string, AudioSource> dictSource;
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
    /// 基调/主调
    /// </summary>
    public int KeyTone { get { return keyTone; } }
    
    public void Init()
    {
        LoadClip(musicalInstrumentName);
        
        int length = transform.childCount;
        dictSource = new Dictionary<string, AudioSource>();
        for (int i = 0; i < length; i++)
        {
            Transform child = transform.GetChild(i);
            dictSource.Add(child.name, child.GetComponent<AudioSource>());
        }
    }

    public void PlayTone(string keyTone,int toneValue)
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

    private void LoadClip(string musicalInstrumentName)
    {
        List<AudioClip> list = new List<AudioClip>();
        string format = Enum.GetName(audioFormat.GetType(), audioFormat);
        for (int i = -1; i <= 7; i++)
        {
            for (int j = 0; j < Tone.toneName.Length; j++)
            {
                string path = "Vsti/" + musicalInstrumentName + "/" + format + "/" + musicalInstrumentName + "_" + Tone
                    .toneName[j] + i.ToString();
                AudioClip audio = Load<AudioClip>(path);
                list.Add(audio);
            }
        }
        if (list.Count > 0)
        {
            audioClip = list.ToArray();
        }
    }

    private T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    private enum AudioFormat
    {
        OGG,
        WAV,
    }
}