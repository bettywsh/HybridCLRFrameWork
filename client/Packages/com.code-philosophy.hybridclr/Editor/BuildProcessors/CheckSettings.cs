﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace HybridCLR.Editor.BuildProcessors
{
    internal class CheckSettings : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            HybridCLRSettings globalSettings = SettingsUtil.HybridCLRSettings;
            if (!globalSettings.enable || globalSettings.useGlobalIl2cpp)
            {
                string oldIl2cppPath = Environment.GetEnvironmentVariable("UNITY_IL2CPP_PATH");
                if (!string.IsNullOrEmpty(oldIl2cppPath))
                {
                    Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", "");
                    Debug.Log($"[CheckSettings] 清除 UNITY_IL2CPP_PATH, 旧值为:'{oldIl2cppPath}'");
                }
            }
            else
            {
                string curIl2cppPath = Environment.GetEnvironmentVariable("UNITY_IL2CPP_PATH");
                if (curIl2cppPath != SettingsUtil.LocalIl2CppDir)
                {
                    Environment.SetEnvironmentVariable("UNITY_IL2CPP_PATH", SettingsUtil.LocalIl2CppDir);
                    Debug.Log($"[CheckSettings] UNITY_IL2CPP_PATH 当前值为:'{curIl2cppPath}'，更新为:'{SettingsUtil.LocalIl2CppDir}'");
                }
            }
            if (!globalSettings.enable)
            {
                return;
            }
            if (PlayerSettings.gcIncremental)
            {
                Debug.LogError($"[CheckSettings] HybridCLR不支持增量式GC，已经自动将该选项关闭");
                PlayerSettings.gcIncremental = false;
            }
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            ScriptingImplementation curScriptingImplementation = PlayerSettings.GetScriptingBackend(buildTargetGroup);
            ScriptingImplementation targetScriptingImplementation = ScriptingImplementation.IL2CPP;
            if (curScriptingImplementation != targetScriptingImplementation)
            {
                Debug.LogError($"[CheckSettings] 当前ScriptingBackend是:{curScriptingImplementation}，已经自动切换为:{targetScriptingImplementation}");
                PlayerSettings.SetScriptingBackend(buildTargetGroup, targetScriptingImplementation);
            }

            var installer = new Installer.InstallerController();
            if (!installer.HasInstalledHybridCLR())
            {
                throw new BuildFailedException($"你没有初始化HybridCLR，请通过菜单'HybridCLR/Installer'安装");
            }

            HybridCLRSettings gs = SettingsUtil.HybridCLRSettings;
            if (((gs.hotUpdateAssemblies?.Length + gs.hotUpdateAssemblyDefinitions?.Length) ?? 0) == 0)
            {
                Debug.LogWarning("[CheckSettings] HybridCLRSettings中未配置任何热更新模块");
            }

        }
    }
}
