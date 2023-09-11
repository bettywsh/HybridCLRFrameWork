using ICSharpCode.SharpZipLib.Zip;
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
            //������manifest
            System.IO.File.Move(ResPack.BuildCreatePath + "/ResCreate.manifest", ResPack.BuildCreatePath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower() + ".manifest");
            System.IO.File.Move(ResPack.BuildCreatePath + "/ResCreate", ResPack.BuildCreatePath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower());
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.Delete(Application.streamingAssetsPath);
            }
            Directory.CreateDirectory(Application.streamingAssetsPath);
            AssetDatabase.Refresh();
            //������streamingAssets    
            //PackFile.CopySourceDirTotargetDir(ResPack.BuildCreatePath.Replace("/app", ""), Application.streamingAssetsPath);
            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create($"{Application.streamingAssetsPath}/{ResConst.RootFolderName.ToLower()}.zip")))
            {
                ZipHelper.Compress($"{ResPack.BuildCreatePath}/{ResConst.RootFolderName.ToLower()}", s);
            }
            AssetDatabase.Refresh();  
        }
        else
        {
            AssetDatabase.Refresh();
            //������manifest
            System.IO.File.Move(ResPack.BuildHotfixPath + "/ResHotfix.manifest", ResPack.BuildHotfixPath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower() + ".manifest");
            System.IO.File.Move(ResPack.BuildHotfixPath + "/ResHotfix", ResPack.BuildHotfixPath + "/" + ResConst.RootFolderName.ToLower() + "/" + ResConst.RootFolderName.ToLower());
        }
    }
}
