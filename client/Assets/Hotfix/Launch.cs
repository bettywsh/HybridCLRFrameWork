using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launch
{
    // Start is called before the first frame update
    public static void Start()
    {
        Debug.unityLogger.logEnabled = AppConst.DebugLog;
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = AppConst.GameFrameRate;

        SoundManager.Instance.Init();
        UIManager.Instance.Init();
        AtlasManager.Instance.Init();
        ResManager.Instance.Init();
        AssetBundleManager.Instance.Init();
        NetworkManager.Instance.Init();
        ConfigManager.Instance.Init();
        ResManager.Instance.LoadAssetAsync("Common", "Font/SourceHanSansCN-NormalSDF.asset", typeof(TMP_FontAsset));
        ResManager.Instance.LoadAssetAsync("Common", "Font/SourceHanSerifCN-BoldSDF.asset", typeof(TMP_FontAsset), (ugo) => {
            //UIManager.Instance.Open<LoginPanel>();
        });
        LoadSceneManager.Instance.LoadScene("Login", false);
        //UIManager.Instance.Open("LoginPanel");
    }

 
}
