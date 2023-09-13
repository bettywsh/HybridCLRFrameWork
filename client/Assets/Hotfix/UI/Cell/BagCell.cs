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
    

    public void SetData(int idx, List<HorseConfigItem> listHorseConfig)
    {
        HorseConfigItem horseConfigItem = listHorseConfig[idx];
        m_Text.text = horseConfigItem.Name;
        ResManager.Instance.LoadAssetAsync(panelName, $"Texture/item/{horseConfigItem.Icon}.png", typeof(Sprite), (ugo) => {
            m_Image.sprite = ugo as Sprite;
        });
    }
}
