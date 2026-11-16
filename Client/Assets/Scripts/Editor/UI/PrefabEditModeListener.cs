using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class PrefabEditModeListener : MonoBehaviour
{
#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // 监听Prefab阶段变化
        PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        PrefabStage.prefabStageClosing += OnPrefabStageClosing;
    }

    private static void OnPrefabStageOpened(PrefabStage stage)
    {
        //Debug.Log($"开始编辑Prefab: {stage.assetPath}");
    }

    private static void OnPrefabStageClosing(PrefabStage stage)
    {
        var rc = stage.prefabContentsRoot.GetComponent<ReferenceCollector>();
        if (rc != null)
        {
            rc.AutoBind();
        }
    }

#endif
}
