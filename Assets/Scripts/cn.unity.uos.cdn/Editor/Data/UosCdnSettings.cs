using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace UosCdn
{
    public class UosCdnSettings : ScriptableObject
    {
        [SerializeField]
        public bool useLatest;
        
        [SerializeField]
        public bool syncWithDelete;

        private static UosCdnSettings m_settings;

        public static UosCdnSettings Settings
        {
            get
            {
                if (m_settings != null)
                {
                    return m_settings;
                }

                if (false == Directory.Exists(Parameters.k_UosCdnSettingsPathPrefix))
                {
                    Directory.CreateDirectory(Parameters.k_UosCdnSettingsPathPrefix);
                }

                m_settings = AssetDatabase.LoadAssetAtPath<UosCdnSettings>(Parameters.k_UosCdnSettingsPath);
                if (m_settings == null)
                {
                    m_settings = ScriptableObject.CreateInstance<UosCdnSettings>();
                    m_settings.useLatest = true;
                    m_settings.syncWithDelete = false;
                    AssetDatabase.CreateAsset(m_settings, Parameters.k_UosCdnSettingsPath);
                    AssetDatabase.SaveAssets();
                }

                return m_settings;

            }
        }
    }
}
