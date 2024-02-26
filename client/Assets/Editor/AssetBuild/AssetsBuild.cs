using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

public class AssetsBuild 
{
    [MenuItem("Build/BuildAndroidAsset", false, 1)]
    public static void BuildAndroid()
    {
        Debug.Log($"��ʼ���� : {BuildTarget.Android}");
        AppConfig appConfig = AssetDatabase.LoadAssetAtPath<AppConfig>("Assets/Resources/AppConfig.asset");
        appConfig.AppVersion = Application.version;
        EditorUtility.SetDirty(appConfig);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        var buildoutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        var streamingAssetsRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
        // ��������
        BuiltinBuildParameters buildParameters = new BuiltinBuildParameters();
        buildParameters.BuildOutputRoot = buildoutputRoot;
        buildParameters.BuildinFileRoot = streamingAssetsRoot;
        buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline.ToString();
        buildParameters.BuildTarget = BuildTarget.Android;
        buildParameters.BuildMode = EBuildMode.ForceRebuild;
        buildParameters.PackageName = "DefaultPackage";
        buildParameters.PackageVersion = appConfig.ResVersion;
        buildParameters.VerifyBuildingResult = true;
        buildParameters.FileNameStyle = EFileNameStyle.HashName;
        buildParameters.BuildinFileCopyOption = EBuildinFileCopyOption.ClearAndCopyAll;
        buildParameters.BuildinFileCopyParams = string.Empty;
        //buildParameters.EncryptionServices = CreateEncryptionInstance();
        buildParameters.CompressOption = ECompressOption.Uncompressed;

        // ִ�й���
        BuiltinBuildPipeline pipeline = new BuiltinBuildPipeline();
        var buildResult = pipeline.Run(buildParameters, true);
        if (buildResult.Success)
        {
            Debug.Log($"�����ɹ� : {buildResult.OutputPackageDirectory}");
        }
        else
        {
            Debug.LogError($"����ʧ�� : {buildResult.ErrorInfo}");
        }
    }
}
