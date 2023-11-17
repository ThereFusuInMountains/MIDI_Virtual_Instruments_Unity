using System;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;

struct CDN
{
    private static string url;
    public static string URL
    {
        get => string.IsNullOrEmpty(url) ? UOS.cdn_url : url;
        set => url = value;
    }
}
class AssetsUpdate : MonoBehaviour
{
    private static AssetsUpdate m_Instance;

    public static AssetsUpdate Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameObject().AddComponent<AssetsUpdate>();
            }

            return m_Instance;
        }
    }

    private IEnumerable keys;

    private AsyncOperationHandle<IResourceLocator> initHand;

    private AsyncOperationHandle<long> downloadSize;

    private AsyncOperationHandle download;

    public event Action OnCompleted;

    public event Action<float> OnUpdate;

    public event Action<string> OnInfo;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        InitAddressbles();
    }

    private void InitAddressbles()
    {
        OnInfo?.Invoke(CDN.URL);
        initHand = Addressables.InitializeAsync();
        initHand.Completed += InitHand_Completed;
    }

    private void InitHand_Completed(AsyncOperationHandle<IResourceLocator> handle)
    {
        GetDownlaodSize();
    }

    private void GetDownlaodSize()
    {
        keys = new string[] { "Base", "Audio" };
        downloadSize = Addressables.GetDownloadSizeAsync(keys);
        downloadSize.Completed += DownloadSize_Completed;
    }

    private void DownloadSize_Completed(AsyncOperationHandle<long> handle)
    {
        Download();
    }

    private void Download()
    {
        var sumSize = downloadSize.Result;
        if (downloadSize.Result > 0)
        {
            download = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
            download.Completed += Download_Completed; ;
        }
        else
        {
            Completed();
        }
    }

    private void Download_Completed(AsyncOperationHandle handle)
    {
        Completed();
    }

    private void Completed()
    {
        OnCompleted?.Invoke();
    }

    private void Update()
    {
        var percentComplete = 0f;
        if (initHand.IsValid())
        {
            percentComplete += initHand.PercentComplete * 0.01f;
        }
        if (downloadSize.IsValid())
        {
            percentComplete += downloadSize.PercentComplete * 0.01f;
        }
        if (download.IsValid())
        {
            percentComplete += download.PercentComplete * 0.98f;
        }
        OnUpdate(percentComplete);
    }
}