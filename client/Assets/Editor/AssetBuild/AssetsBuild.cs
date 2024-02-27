using HybridCLR.Editor.Commands;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

public class AssetsBuild
{
    public static List<string> AssembliesPostIl2CppStrip = new List<string>{"mscorlib.dll", "System.Core.dll", "System.dll" };
    public static List<string> HotUpdateDlls = new List<string> { "Hotfix.dll" };

    [MenuItem("Build/BuildAndroidAsset", false, 1)]
    public static void BuildAndroid()
    {
        Debug.Log($"��ʼ���� : ��٢dll");
        PrebuildCommand.GenerateAll();
        AssetDatabase.Refresh();
        string SourceDir = $"{Application.dataPath}/../HybridCLRData/HotUpdateDlls/{BuildTarget.Android.ToString()}/";
        string TargetDir = $"{Application.dataPath}/App/Dll/";
        foreach (string file in HotUpdateDlls)
        {
            File.Copy(SourceDir+file, TargetDir+file+ ".bytes",true);
        }

        SourceDir = $"{Application.dataPath}/../HybridCLRData/AssembliesPostIl2CppStrip/{BuildTarget.Android.ToString()}/";
        foreach (string file in AssembliesPostIl2CppStrip)
        {
            File.Copy(SourceDir + file, TargetDir + file + ".bytes", true);
        }
        AssetDatabase.Refresh();

        Debug.Log($"��ʼ���� : {BuildTarget.Android}");
        AppConfig appConfig = AssetDatabase.LoadAssetAtPath<AppConfig>("Assets/Resources/AppConfig.asset");
        //appConfig.AppVersion = Application.version;
        //EditorUtility.SetDirty(appConfig);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();

        var buildoutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        var streamingAssetsRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
        Debug.LogError(buildoutputRoot);
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
        AssetDatabase.Refresh();

        Debug.Log($"д��汾�ļ�");
        string verPath = $"{buildoutputRoot}/{BuildTarget.Android.ToString()}/DefaultPackage/{appConfig.ResVersion}/ver.txt";
        File.WriteAllText(verPath, Application.version);
    }
}
