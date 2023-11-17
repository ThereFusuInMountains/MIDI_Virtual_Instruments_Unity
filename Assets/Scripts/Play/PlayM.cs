using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayM : MonoBehaviour
{
    public PlayData data;
    public void Init()
    {
        data = new PlayData();
    }
}
[System.Serializable]
public class PlayData
{
    public Notation notation;
    public bool MIDI;
    public Schema schema;
    public MusicalInstrument musicalInstrument;

    public PlayData(Notation notation,bool MIDI,Schema schema,MusicalInstrument musicalInstrument)
    {
        this.notation = notation;
        this.MIDI = MIDI;
        this.schema = schema;
        this.musicalInstrument = musicalInstrument;
    }
    public PlayData()
    {
        this.notation = Notation.NumberedMusicalNotation;
        this.MIDI = false;
        this.schema = Schema.Study;
        this.musicalInstrument = null;
    }
}
/// <summary>
/// 键盘模式
/// </summary>
public enum Notation
{
    NumberedMusicalNotation,
    ChineseMusic,
}
/// <summary>
/// 演奏模式
/// </summary>
public enum Schema
{
    /// <summary>
    /// 练习
    /// </summary>
    Study,
    /// <summary>
    /// 演奏
    /// </summary>
    Play,
}
