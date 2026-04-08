using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEditor.Build.Reporting;
using UnityEditor.Android;
using System.IO;
using System.Xml;

public class CustomBuildProcesser : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get { return 0; } }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        string manifestPath = Path.Combine(path, "src/main/AndroidManifest.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(manifestPath);

        // ÐÞ¸Ä Activity ½Úµã
        XmlNode activityNode = doc.SelectSingleNode("manifest/application/activity[@android:name='com.mtgame.com.MainActivity']",
            GetNamespaceManager(doc));
        if (activityNode != null)
        {
            XmlAttribute activityHardwareAttr = doc.CreateAttribute("android", "hardwareAccelerated", "http://schemas.android.com/apk/res/android");
            activityHardwareAttr.Value = "true";
            activityNode.Attributes.Append(activityHardwareAttr);
        }

        doc.Save(manifestPath);
    }

    private XmlNamespaceManager GetNamespaceManager(XmlDocument doc)
    {
        XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
        nsManager.AddNamespace("android", "http://schemas.android.com/apk/res/android");
        return nsManager;
    }
}
