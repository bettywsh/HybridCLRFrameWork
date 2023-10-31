using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BagCell : BaseCell
{
    public TextMeshProUGUI m_Text;
    public Image m_Image;
    

    public async void SetData(int idx, HorseConfigItem horseConfigItem)
    {
        m_Text.text = horseConfigItem.Name;
        await ResManager.Instance.SceneLoadAssetAsync<Sprite>($"Assets/App/Texture/item/{horseConfigItem.Icon}.png");
    }
}
