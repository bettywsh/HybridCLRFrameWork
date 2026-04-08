using ICSharpCode.SharpZipLib.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectHelper
{
    public static Transform Instantiate(Transform parent, GameObject children)
    {
        var go = Object.Instantiate<GameObject>(children).transform;
        SetParent(parent, go);
        return go;
    }

    public static Transform SetParent(Transform parent, Transform go)
    {
        go.SetParent(parent, false);        
        go.localPosition = Vector3.zero;
        go.localScale = Vector3.one;
        go.localEulerAngles = Vector3.zero;
        return go;
    }

    public static async void SetGrey(Transform transform, bool isGrey)
    {
        var images = transform.GetComponentsInChildren<Image>();
        foreach(var image in images)
        {
            if (isGrey)
                image.material = await ResManager.Instance.SceneLoadAssetAsync<Material>("Assets/App/Material/ImageGrey.mat");
            else
                image.material = null;
        }

        var texts = transform.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            text.fontMaterial.SetFloat("_Grey", isGrey == true ? 1: 0);
        }

        var buttons = transform.GetComponentsInChildren<UButton>();
        foreach (var button in buttons)
        {
            button.enabled = !isGrey;
        }
    }


    public static void SetMaterials(Renderer renderer, Material material1)
    {
        var materials = new Material[]{ new Material(material1) };
        renderer.materials = materials;
    }
    public static void SetMaterials(Renderer renderer, Material material1, Material material2)
    {
        var materials = new Material[] { new Material(material1), new Material(material2) };
        renderer.materials = materials;
    }
}
