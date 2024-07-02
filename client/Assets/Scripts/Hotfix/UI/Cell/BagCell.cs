using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BagCell : CellBase
{

    public async void OnOpen(cfg.Horse horseConfigItem)
    {
        referenceData["txtTitle"].tmptxtValue.text = horseConfigItem.Name;
        referenceData["imgIcon"].imgValue.sprite = await ResManager.Instance.SceneLoadAssetAsync<Sprite>($"Assets/App/Texture/item/{horseConfigItem.Icon}.png");
    }
}
