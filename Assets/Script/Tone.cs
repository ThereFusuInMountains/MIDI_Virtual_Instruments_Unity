using System.Collections.Generic;

public static class Tone
{
    public static string[] toneName = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
    public static Dictionary<string, int> dict;

    static Tone()
    {
        InitDict();
    }

    private static void InitDict()
    {
        dict = new Dictionary<string, int>();
        int value = 0;
        for (int i = -1; i <= 7; i++)
        {
            for (int j = 0; j < toneName.Length; j++)
            {
                dict.Add(toneName[j] + i.ToString(), value);
                value++;
            }
        }
    }
}
