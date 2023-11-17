using LitJson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;

class App : MonoBehaviour
{
    public Text infoText;
    public Text sizeText;
    public Slider slider;
    public Button loadCDN;
    public Button resetCDN;
    public Button initAddress;
    public Dropdown cdnDropdown;
    public AssetReference minScene;
    public AssetReferenceT<TextAsset> cdnJson;

    private Dictionary<string, string> CDNDict;

    private void Start()
    {
        cdnDropdown.Hide();
        loadCDN.onClick.AddListener(LoadCDNJson);
        cdnDropdown.onValueChanged.AddListener(OnChangedCDN);
        resetCDN.onClick.AddListener(ResetCDN);
        initAddress.onClick.AddListener(AddressablesInit);
    }

    private void AddressablesInit()
    {
        Addressables.InitializeAsync().Completed += AddressablesInit_Completed;
    }

    private void AddressablesInit_Completed(AsyncOperationHandle<IResourceLocator> handle)
    {
        UpdateAssets();
        Addressables.Release(handle);
    }

    private void UpdateAssets()
    {
        AssetsUpdate.Instance.OnCompleted += AssetsUpdateCompleted;
        AssetsUpdate.Instance.OnUpdate += RefreshProgress;
        AssetsUpdate.Instance.OnInfo += RefreshInfo;
    }

    private void ResetCDN()
    {
        cdnDropdown.Hide();
        Caching.ClearCache();
    }

    private void LoadCDNJson()
    {
        cdnJson.LoadAssetAsync<TextAsset>().Completed += LoadCDNJsonCompleted;
    }

    private void LoadCDNJsonCompleted(AsyncOperationHandle<TextAsset> textAsset)
    {
        var json = textAsset.Result.text;
        CDNDict = JsonMapper.ToObject<Dictionary<string, string>>(json);
        cdnDropdown.ClearOptions();
        var count = CDNDict.Count;
        var arr = new string[count];
        CDNDict.Keys.CopyTo(arr, 0);
        var keys = new List<string>(arr);
        cdnDropdown.AddOptions(keys);
        cdnDropdown.Show();
        Addressables.Release(textAsset);
    }

    private void OnChangedCDN(int index)
    {
        var key = cdnDropdown.options[index].text;
        if (!CDNDict.TryGetValue(key, out var url))
        {
            return;
        }
        CDN.URL = url;
        RefreshInfo(CDN.URL);
    }

    private void RefreshInfo(string msg)
    {
        infoText.text = msg;
    }

    private void AssetsUpdateCompleted()
    {
        Addressables.LoadSceneAsync(minScene);
    }

    private void RefreshProgress(float percent)
    {
        slider.value = percent;
        sizeText.text = 100 * percent + "%";
        Debug.Log(percent);
    }
}