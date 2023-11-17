using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildWithReport : IPostprocessBuildWithReport,IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
       
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "Other/user.keystore";
        PlayerSettings.Android.keyaliasPass = "fwqtest";
        PlayerSettings.Android.keystorePass = "fwqtest";
        PlayerSettings.Android.keyaliasName = "test";
    }
}
