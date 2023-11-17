using System;
using System.Collections.Generic;
using UnityEngine;

namespace UosCdn
{
    [Serializable]
    public class Parameters
    {
        //constant parameters
        public static string apiHost = "https://a.unity.cn/";
        public static Dictionary<string, string> proxyHost = new Dictionary<string, string> {
            {"cos","https://a.unity.cn/"},
            { "BlueCloud","https://a.unity.cn/"}
        };
        public static string contentType = "application/json";
        public static string k_UosCdnSettingsPathPrefix = "Assets/UnityOnlineServiceData/";
        public static string k_UosSettingsPath = "Assets/UnityOnlineServiceData/UosSettings.asset";
        public static string k_UosCdnSettingsPath = "Assets/UnityOnlineServiceData/UosCdnSettings.asset";
        public static string k_UploadPartStatusPathPrefix = "Assets/UnityOnlineServiceData/";
        public static string k_UploadPartStatusFile = "Assets/UnityOnlineServiceData/unfinishedUploads.json";

        public static string CosAppId = "1301029430";
        public static string CosRegion = "ap-shanghai";
        public static string CosBucket = "asset-streaming-1301029430";
        public static int maxRetries = 1;
        public static int createMultiCount = 20;
        public static int deleteMultiCount = 20;

        //static parameters
        public static List<string> ignoreFiles = new List<string>()
        {
            ".DS_Store"
        };
        
        public static string uosAppId = UosAppInfo.getUosAppId();
        public static string uosAppSecret = UosAppInfo.getUosAppSecret();
        public static string oldUosAppId = "";
        public static string oldUosAppSecret = "";
        public static string projectGuid = UosAppInfo.getProjectGuid();
        public static string backend = UosAppInfo.getbackend();
        public static bool useOverseaConfig = UosAppInfo.getUseOversea();

        public static int countPerpage = 10;

        public static void ApplyConfigByEnvironment()
        {
            apiHost = "https://a.unity.cn/";
            proxyHost["cos"] = "https://a.unity.cn/";
            CosRegion = "ap-shanghai";
            CosBucket = "asset-streaming-1301029430";
        }
    }
}