using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

[Cell]
public class BagCell : CellBase
{

    public async void SetData(cfg.Horse horseConfigItem)
    {
        referenceCollector.Get("txtTitle").tmptxtValue.text = horseConfigItem.Name;
        referenceCollector.Get("imgIcon").imgValue.sprite = await ResManager.Instance.SceneLoadAssetAsync<Sprite>($"Assets/App/Texture/item/{horseConfigItem.Icon}.png", cancellationTokenSource.Token);
    }
}
