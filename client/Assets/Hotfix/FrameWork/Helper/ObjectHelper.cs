using ICSharpCode.SharpZipLib.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHelper
{
    public static Transform Instantiate(Transform parent, GameObject children)
    {
        Transform go = GameObject.Instantiate<GameObject>(children).transform;
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

    public static void SetGrey(Transform transform, bool isGrey)
    {
        Image[] images = transform.GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            if (isGrey)
                image.material = ResManager.Instance.LoadAsset(LoadSceneManager.Instance.name, "Material/ImageGrey.mat", typeof(Material)) as Material;
            else
                image.material = null;
        }

        Button[] buttons = transform.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            if (isGrey)
                button.enabled = false;
            else
                button.enabled = true;
        }
    }


    public static void SetMaterials(Renderer renderer, Material material1)
    {
        Material[] materials = new Material[]{ new Material(material1) };
        renderer.materials = materials;
    }
    public static void SetMaterials(Renderer renderer, Material material1, Material material2)
    {
        Material[] materials = new Material[] { new Material(material1), new Material(material2) };
        renderer.materials = materials;
    }
}
