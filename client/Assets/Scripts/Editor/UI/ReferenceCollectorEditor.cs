using System.IO;
using System.Text;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReferenceCollector))]
public class ReferenceCollectorEditor : OdinEditor
{
    private const string PanelDir = "/Scripts/Hotfix/UI/Panel";
    private const string PanelTempletePath = "Assets/Scripts/Editor/UI/TempPanel.bytes";

    private const string DataDir = "/Scripts/Hotfix/Data";
    private const string DataTempletePath = "Assets/Scripts/Editor/UI/TempData.bytes";

    private const string SubPanelDir = "/Scripts/Hotfix/UI/SubPanel";
    private const string SubPanelTempletePath = "Assets/Scripts/Editor/UI/TempSubPanel.bytes";

    private const string CellDir = "/Scripts/Hotfix/UI/Cell";
    private const string CellTempletePath = "Assets/Scripts/Editor/UI/TempCell.bytes";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var collector = (ReferenceCollector)target;
        if (collector == null)
            return;

        string objName = collector.transform.name;
        bool isPanel = objName.Contains("Panel") && !objName.Contains("SubPanel");
        bool isSubPanel = objName.Contains("SubPanel");
        bool isCell = objName.Contains("Cell");

        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;

        if (isPanel)
        {
            if (GUILayout.Button("创建Panel.cs", GUILayout.Height(30)))
                CreateCs(PanelDir, PanelTempletePath, objName, objName, "创建Panel成功!!!");
            if (GUILayout.Button("创建Data.cs", GUILayout.Height(30)))
                CreateCs(DataDir, DataTempletePath, objName.Replace("Panel", "Data"), objName, "创建Data成功!!!");
        }

        if (isSubPanel)
        {
            if (GUILayout.Button("创建SubPanel.cs", GUILayout.Height(30)))
                CreateCs(SubPanelDir, SubPanelTempletePath, objName, objName, "创建SubPanel成功!!!");
        }

        if (isCell)
        {
            if (GUILayout.Button("创建Cell.cs", GUILayout.Height(30)))
                CreateCs(CellDir, CellTempletePath, objName, objName, "创建Cell成功!!!");
        }

        GUI.backgroundColor = oldColor;
    }

    private static void CreateCs(string dir, string templetePath, string fileName, string className, string successMsg)
    {
        var fullFilePath = EditorUtility.SaveFilePanel("Please select a folder to create", Application.dataPath + dir, fileName, "cs");

        if (string.IsNullOrEmpty(fullFilePath))
            return;

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("文件已存在");
            return;
        }

        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(templetePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", className);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", successMsg, "知道了");
    }
}
