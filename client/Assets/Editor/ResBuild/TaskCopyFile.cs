using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
public class TaskCopyFile : ITask
{
    public void Run(PackSetting packSetting)
    {
        if (!packSetting.IsHotfix)
        {
            AssetDatabase.Refresh();
            //重命名manifest
            System.IO.File.Move(ResPack.BuildCreatePath + "/ResCreate.manifest", ResPack.BuildCreatePath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower() + ".manifest");
            System.IO.File.Move(ResPack.BuildCreatePath + "/ResCreate", ResPack.BuildCreatePath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower());
            AssetDatabase.Refresh();
            //拷贝到streamingAssets
            PackFile.CopySourceDirTotargetDir(ResPack.BuildCreatePath.Replace("/app", ""), Application.streamingAssetsPath);
            AssetDatabase.Refresh();
        }
        else
        {
            AssetDatabase.Refresh();
            //重命名manifest
            System.IO.File.Move(ResPack.BuildHotfixPath + "/ResHotfix.manifest", ResPack.BuildHotfixPath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower() + ".manifest");
            System.IO.File.Move(ResPack.BuildHotfixPath + "/ResHotfix", ResPack.BuildHotfixPath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower());
        }
    }
}
