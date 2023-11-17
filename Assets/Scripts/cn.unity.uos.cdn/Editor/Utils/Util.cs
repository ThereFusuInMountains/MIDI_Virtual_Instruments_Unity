using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Utils;
using UnityEditor;
using UnityEngine;

namespace UosCdn
{
    public class Util
    {
        public static Dictionary<string, string> contentTypeMapping = new Dictionary<string, string>
        {
            {".hash", "application/octet-stream"},
            {".json", "text/plain; charset=utf-8"},
            {".bundle", "application/octet-stream"}
        };
        
        public static Dictionary<string, EntryInfo> getLocalFiles(string rootPath)
        {
            Dictionary<string, EntryInfo> localFiles = new Dictionary<string, EntryInfo>();
            DirectoryInfo root = new DirectoryInfo(rootPath);
            rootPath = root.FullName.Replace("\\", "/");
            FileInfo[] files = root.GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo file in files)
            {
                string fullPath = file.FullName.Replace("\\", "/");
                string path = getRelativePath(rootPath, fullPath);
                long size = file.Length;
                string contentType = getContentTypeFromExtension(path);
                string hash = Util.getFiletHash(file.FullName);

                if (Parameters.ignoreFiles.Contains(file.Name))
                {
                    continue;
                }

                EntryInfo entry = new EntryInfo(fullPath, path, hash, size, contentType);
                localFiles.Add(path, entry);
            }

            return localFiles;
        }

        public static EntryInfo getEntryInfoFromLocalFile(string filepath)
        {
            FileInfo file = new FileInfo(filepath);

            string fullPath = file.FullName.Replace("\\", "/");
            string path = file.Name;
            long size = file.Length;
            string contentType = getContentTypeFromExtension(path);
            string hash = getFiletHash(file.FullName);

            return new EntryInfo(fullPath, path, hash, size, contentType);

        }

        public static string getRelativePath(string rootPath, string fullName)
        {
            return fullName.StartsWith(rootPath, StringComparison.Ordinal) ? fullName.Substring(rootPath.Length + 1) : "";
        }


        public static string getContentTypeFromExtension(string path)
        {
            string ext = Path.GetExtension(path);
            if (contentTypeMapping.ContainsKey(ext))
            {
                return contentTypeMapping[ext];
            }
            else
            {
                return "application/octet-stream";
            }
        }
        
        public static string getFiletHash(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static ProjectInfo getProjectInfo()
        {
            if (string.IsNullOrEmpty(Parameters.uosAppId) || string.IsNullOrEmpty(Parameters.uosAppSecret))
            {
                return null;
            }

            try
            {
                string responseBody = HttpUtil.getHttpResponse(Parameters.apiHost + "api/v1/users/me/uos/", "GET");
                ProjectInfo projectInfo = JsonUtility.FromJson<ProjectInfo>(responseBody);
                // Debug.Log(responseBody);
                Debug.Log(string.Format("Refresh project info successfully : {0}", projectInfo.UnityProjectGuid));
                // Debug.Log(string.Format("Current CDN Provider : {0}", projectInfo.Provider));
                return projectInfo;
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("Refresh project info failed : {0}", e.Message));
                return null;
            }
        }
        
        public static bool checkUosAuth()
        {
            if (string.IsNullOrEmpty(Parameters.uosAppId) || string.IsNullOrEmpty(Parameters.uosAppSecret))
            {
                EditorUtility.DisplayDialog("Warning", "Please Set UOS Auth at Edit -> Project Settings -> Unity Online Service!", "OK");
                return false;
            }

            return true;
        }

        public static string getHeader(WebHeaderCollection headers, string key)
        {
            for (int i = 0; i < headers.Keys.Count; i++)
            {
                if (headers.Keys[i].Equals(key))
                {
                    return headers[i];
                }
            }
            return "";
        }
    }
}
