using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEditor;

public class TaskDll : ITask
{
    private List<string> HotfixMetaAssemblyFiles { get; } = new List<string>()
    {
        "Hotfix.dll",
    };
    private List<string> AOTMetaAssemblyFiles { get; } = new List<string>()
    {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",
    };
    public void Run(PackSetting packSetting)
    {
        HybridCLR.Editor.Commands.PrebuildCommand.GenerateAll();
        AssetDatabase.Refresh();
        string dllDir = ResPath.AppFullPath + "/Dll";
        if (Directory.Exists(dllDir))
        {
            Directory.Delete(dllDir, true);
            AssetDatabase.Refresh();
            Directory.CreateDirectory(dllDir);
        }
        else {
            Directory.CreateDirectory(dllDir);
        }

        for (int i = 0; i < HotfixMetaAssemblyFiles.Count; i++)
        {
            string file = Application.dataPath.Replace("Assets", "") + $"HybridCLRData/HotUpdateDlls/Android/"+ HotfixMetaAssemblyFiles[i];
            string copyFile = dllDir + "/" + HotfixMetaAssemblyFiles[i] + ".bytes";
            File.Copy(file, copyFile);
        }
        for (int i = 0; i < AOTMetaAssemblyFiles.Count; i++)
        {
            string file = Application.dataPath.Replace("Assets", "") + $"HybridCLRData/AssembliesPostIl2CppStrip/Android/" + AOTMetaAssemblyFiles[i];
            string copyFile = dllDir + "/" + AOTMetaAssemblyFiles[i] + ".bytes";
            File.Copy(file, copyFile);
        }
    }

}
