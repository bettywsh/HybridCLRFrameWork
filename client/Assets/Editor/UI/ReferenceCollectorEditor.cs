using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ReferenceCollector))]
public class ReferenceCollectorEditor : BaseEditor
{
    ReferenceCollector m_ReferenceCollector;
    ReorderableList m_ReordList;
    void OnEnable()
    {
        m_ReferenceCollector = target as ReferenceCollector;
        m_ReordList = this.CreateItemList();
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        m_ReordList.DoLayoutList();
        if (GUILayout.Button("Auto bind"))
        {
            Undo.RecordObject(m_ReferenceCollector, "Auto Bind");
            m_ReferenceCollector.AutoBind();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private ReorderableList CreateItemList()
    {
        void OnAddItem(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("name").stringValue = string.Empty;
            element.FindPropertyRelative("referenceData").objectReferenceValue = null;
        }

        void OnRemoveItem(ReorderableList list)
        {
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the reference?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);

                if (m_ReordList.index == m_ReferenceCollector.data.Count - 1)
                {
                    serializedObject.FindProperty("m_selectedIndex").intValue = m_ReordList.index = m_ReordList.index - 1;
                }
            }
        }

        void OnSelectItem(ReorderableList list)
        {
            serializedObject.FindProperty("m_selectedIndex").intValue = list.index;
            serializedObject.ApplyModifiedProperties();
            GUI.changed = true;
        }

        void OnReorderItem(ReorderableList list)
        {
            Repaint();
        }

        ReorderableList reordList = CreateRecordList(serializedObject, "data", "Reference List", OnReorderItem, OnSelectItem, OnAddItem, OnRemoveItem);
        reordList.index = serializedObject.FindProperty("m_selectedIndex").intValue;
        reordList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            if (reordList.elementHeight == 0) return;
            var e = reordList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 160, EditorGUIUtility.singleLineHeight),
                e.FindPropertyRelative("name"), GUIContent.none);

            EditorGUI.PropertyField(new Rect(rect.x + 160, rect.y, 160, EditorGUIUtility.singleLineHeight),
                e.FindPropertyRelative("referenceData"), GUIContent.none);
        };
        return reordList;
    }
}
