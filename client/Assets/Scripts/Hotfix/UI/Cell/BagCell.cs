using UnityEngine;

[Cell]
public class BagCell : CellBase
{

    public async void SetData(cfg.Horse horseConfigItem)
    {
        GetUI("txtTitle").tmptxtValue.text = horseConfigItem.Name;
        GetUI("imgIcon").imgValue.sprite = await ResManager.Instance.SceneLoadAssetAsync<Sprite>($"Assets/App/Texture/item/{horseConfigItem.Icon}.png", cancellationTokenSource.Token);
    }
}
