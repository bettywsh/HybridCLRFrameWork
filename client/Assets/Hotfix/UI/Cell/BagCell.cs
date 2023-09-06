using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagCell : MonoBehaviour
{
    public TextMeshProUGUI m_Text;
    public Image m_Image;

    public void SetData(int idx, List<HorseConfigItem> listHorseConfig)
    {
        m_Text.text = listHorseConfig[idx].Name;
    }
}
