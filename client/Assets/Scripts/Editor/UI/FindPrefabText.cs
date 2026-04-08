using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FindPrefabText : MonoBehaviour
{
    [MenuItem("Assets/FindPrefabTextAndTextMeshPro", false, 100)]
    static void FindPrefabTextAndTextMeshPro()
    {
        // 获取当前选中的Prefab
        GameObject selectedPrefab = Selection.activeGameObject;
        var texts = selectedPrefab.GetComponentsInChildren<Text>();
        foreach (var text in texts)
        {
            Debug.LogError(text.gameObject.name + "包含Text组件");
        }
        var images = selectedPrefab.GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            string path = AssetDatabase.GetAssetPath(image.sprite);
            if (!path.Contains("/Common/") && !path.Contains($"/{selectedPrefab.name}/"))
            {
                Debug.LogError(path + "该路径不合法，节点为" + image.gameObject.name);
            }

        }
        var tmpuguis = selectedPrefab.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var tmpugui in tmpuguis)
        {
            string path = AssetDatabase.GetAssetPath(tmpugui.font);
            if (!path.Contains("/Font/PuHuiTi SDF.asset"))
            {
                Debug.LogError(path + "该路径不合法，节点为" + tmpugui.gameObject.name);
            }
        }
        var tmpinputs = selectedPrefab.GetComponentsInChildren<TMP_InputField>();
        foreach (var tmpinput in tmpinputs)
        {
            string path = AssetDatabase.GetAssetPath(tmpinput.fontAsset);
            if (!path.Contains("/Font/PuHuiTi SDF.asset"))
            {
                Debug.LogError(path + "该路径不合法，节点为" + tmpinput.gameObject.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
