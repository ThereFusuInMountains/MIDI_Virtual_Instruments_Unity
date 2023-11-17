using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace UosCdn
{
    [Serializable]
    public class UosCdnWindow : EditorWindow
    {

        [MenuItem("Window/Unity Online Service/CDN/Manager", priority = 10)]
        internal static void Init()
        {
            var window = GetWindow<UosCdnManager>();
            window.titleContent = new GUIContent("UOS CDN");
            window.Show();
        }

        [MenuItem("Window/Unity Online Service/CDN/Settings", priority = 11)]
        internal static void ShowSettingsInspector()
        {
            var setting = UosCdnSettings.Settings;
            if (setting == null)
            {
                Debug.LogWarning("Attempting to inspect default Addressables Settings, but no settings file exists. Open 'Window/Asset Management/Addressables/Groups' for more info.");
            }
            else
            {
                EditorApplication.ExecuteMenuItem("Window/General/Inspector");
                EditorGUIUtility.PingObject(setting);
                Selection.activeObject = setting;
            }
        }
        
        [MenuItem("Window/Unity Online Service/CDN/Go to Dashboard", priority = 30)]
        internal static void GoToDashbaoard()
        {
            Application.OpenURL("https://uos.unity.cn/");
        }
    }
}
