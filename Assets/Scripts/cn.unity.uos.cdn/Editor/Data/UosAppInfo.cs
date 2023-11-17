using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace UosCdn
{
    public class UosAppInfo : ScriptableObject
    {
        [SerializeField] 
        private string uosAppId;

        [SerializeField] 
        private string uosAppSecret;

        [SerializeField] 
        private string projectGuid;

        [SerializeField]
        private string backend;

        [SerializeField]
        private bool oversea;

        internal static UosAppInfo GetOrCreateSetting()
        {
            if (false == Directory.Exists(Parameters.k_UosCdnSettingsPathPrefix))
            {
                Directory.CreateDirectory(Parameters.k_UosCdnSettingsPathPrefix);
            }
            var setting = AssetDatabase.LoadAssetAtPath<UosAppInfo>(Parameters.k_UosSettingsPath);
            if (setting == null)
            {
                setting = ScriptableObject.CreateInstance<UosAppInfo>();
                setting.uosAppId = "";
                setting.uosAppSecret = "";
                setting.projectGuid = "";
                setting.backend = "";
                setting.oversea = false;
                AssetDatabase.CreateAsset(setting, Parameters.k_UosSettingsPath);
                AssetDatabase.SaveAssets();
            }

            setting.oversea = false;
            return setting;
        }

        internal static void SaveSetting(string uosAppId, string uosAppSecret)
        {
            var setting = AssetDatabase.LoadAssetAtPath<UosAppInfo>(Parameters.k_UosSettingsPath);
            setting.uosAppId = uosAppId;
            setting.uosAppSecret = uosAppSecret;
            EditorUtility.SetDirty(setting);
        }

        public static string getUosAppId()
        {
            return GetOrCreateSetting().uosAppId;
        }

        public static string getUosAppSecret()
        {
            return GetOrCreateSetting().uosAppSecret;
        }

        internal static void SaveSetting(bool oversea)
        {
            var setting = AssetDatabase.LoadAssetAtPath<UosAppInfo>(Parameters.k_UosSettingsPath);
            setting.oversea = oversea;
            EditorUtility.SetDirty(setting);
        }

        public static bool getUseOversea()
        {
            return GetOrCreateSetting().oversea;
        }

        public static void SaveProjectGuid(string projectGuid) {
            var setting = AssetDatabase.LoadAssetAtPath<UosAppInfo>(Parameters.k_UosSettingsPath);
            setting.projectGuid = projectGuid;
            EditorUtility.SetDirty(setting);
        }

        public static string getbackend() {
            return GetOrCreateSetting().backend;
        }

        public static void Savebackend(string backend)
        {
            var setting = AssetDatabase.LoadAssetAtPath<UosAppInfo>(Parameters.k_UosSettingsPath);
            setting.backend = backend;
            EditorUtility.SetDirty(setting);
        }

        public static string getProjectGuid()
        {
            return GetOrCreateSetting().projectGuid;
        }

        static class UosCdnSettingsIMGUIRegister
        {
            [SettingsProvider]
            public static SettingsProvider CreateUosCdnSettingsProvider()
            {
                var provider = new SettingsProvider("Project/Unity Online Service", SettingsScope.Project)
                {
                    label = "Unity Online Service",
                    guiHandler = (searchContext) =>
                    {
                        var setting = UosAppInfo.GetOrCreateSetting();
                        setting.uosAppId = EditorGUILayout.TextField("App Id", setting.uosAppId);
                        setting.uosAppSecret = EditorGUILayout.TextField("App Secret", setting.uosAppSecret);
                        if (!setting.uosAppId.Equals(Parameters.uosAppId) || !setting.uosAppSecret.Equals(Parameters.uosAppSecret))
                        {
                            UosAppInfo.SaveSetting(setting.uosAppId, setting.uosAppSecret);
                            Parameters.uosAppId = setting.uosAppId;
                            Parameters.uosAppSecret = setting.uosAppSecret;
                            ProjectInfo projectInfo = Util.getProjectInfo();
                            string projectGuid = "";
                            string backend = "";
                            if (projectInfo != null)
                            {
                                projectGuid = projectInfo.UnityProjectGuid;
                                backend = projectInfo.Provider;
                            }
                            UosAppInfo.SaveProjectGuid(projectGuid);
                            Parameters.projectGuid = projectGuid;
                            UosAppInfo.Savebackend(backend);
                            Parameters.backend = backend;
                            if (backend.Equals("cos"))
                            {
                                TemporaryAuth.refresh();
                            }
                        }

                        // setting.oversea = EditorGUILayout.Toggle("Use Oversea Config", setting.oversea);
                        setting.oversea = false;
                        if (!setting.oversea.Equals(Parameters.useOverseaConfig))
                        {
                            UosAppInfo.SaveSetting(setting.oversea);
                            Parameters.useOverseaConfig = setting.oversea;
                            Parameters.ApplyConfigByEnvironment();
                        }

                        if (GUILayout.Button("Go To Unity Online Service Dashboard."))
                        {
                            Application.OpenURL("https://uos.unity.cn/");
                        }
                    },
                    keywords = new HashSet<string>(new[] { "UOS" })
                };
                
                AssetDatabase.SaveAssets();
                return provider;
            }
        }
    }
}