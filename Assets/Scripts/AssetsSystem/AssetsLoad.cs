using UnityEngine.AddressableAssets;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using System.Collections;
using System.Collections.Generic;

public class AssetsLoad
{
    public static void Load<TObject>(string key, Action<TObject> completed, Action<string> destroyed = null) where TObject : Object
    {
        var handle = Addressables.LoadAssetAsync<TObject>(key);

        handle.Completed += (handle_Completed) => { completed?.Invoke(handle_Completed.Result); };

        handle.Destroyed += (handle_Destroyed) => { destroyed?.Invoke(key); };
    }

    /// <summary>
    /// 通过标签加载多个取并集
    /// </summary>
    public static void Load<TObject>(IEnumerable keys, Action<IList<TObject>> completed, Action<TObject> callback = null, Action<IEnumerable> destroyed = null, bool Union = false)
    {
        var handle = Addressables.LoadAssetsAsync<TObject>(keys, callback, Union ? Addressables.MergeMode.Union : Addressables.MergeMode.Intersection, true);
        handle.Completed += (handle_Completed) =>
        {
            completed?.Invoke(handle_Completed.Result);
        };
        handle.Destroyed += (handle_Destroyed) => { destroyed?.Invoke(keys); };
    }
}

public enum AddressablesLable
{
    Default = 1,
    Base = 1 << 1,
    Chinese = 1 << 2,
    MusicalInstrument = 1 << 3,
    Prefab = 1 << 4,
    Audio = 1 << 5,
}
