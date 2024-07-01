using HybridCLR.Editor.Commands;
using System;
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
    public static void BuildAndroidAsset()
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
        // ��������
        BuiltinBuildParameters buildParameters = new BuiltinBuildParameters();
        buildParameters.BuildOutputRoot = buildoutputRoot;
        buildParameters.BuildinFileRoot = streamingAssetsRoot;
        buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline.ToString();
        buildParameters.BuildTarget = BuildTarget.Android;
        buildParameters.BuildMode = EBuildMode.ForceRebuild;
        buildParameters.PackageName = "DefaultPackage";
        buildParameters.PackageVersion = appConfig.ResVersion.ToString();
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

        Debug.Log($"д��汾�ļ�");
        string verPath = $"{buildoutputRoot}/{BuildTarget.Android.ToString()}/DefaultPackage/{appConfig.ResVersion}/ver.txt";
        File.WriteAllText(verPath, appConfig.AppVersion.ToString());

        AssetDatabase.Refresh();
    }

    [MenuItem("Build/BuildAndroidApk", false, 1)]
    public static void BuildAndroidApk() {
        BuildAndroidAsset();

        var currentApkName = DateTime.Now.ToString("MM_dd_HH_mm_ss");
        var apkDir = string.Format("{0}/_APK/{1}", $"{Application.dataPath}/../", currentApkName);
        if (!Directory.Exists(apkDir))
            Directory.CreateDirectory(apkDir);
        string toPath = string.Format("{0}/mjdmx_{1}.apk", apkDir, currentApkName);

        string[] scenes = { "Assets/App/Scene/Start.unity",
                            "Assets/App/Scene/Main.unity",
                            "Assets/App/Scene/Battle.unity",
                            "Assets/App/Scene/Login.unity" };

        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        BuildPipeline.BuildPlayer(scenes, toPath, BuildTarget.Android, BuildOptions.CompressWithLz4HC);

        Debug.Log("APK ���");
    }
}
