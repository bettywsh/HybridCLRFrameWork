using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    public enum EquipmentQuality
    {
        Normal = 0,
        Good = 1,
        Excellent = 2,
        Epic = 3,
        Legend=4,
        Mythical = 5,
    }
    public enum EquipQuality
    {
        Normal=1,
        Good,
        Excellent,
        Epic,
        Legend,
        Mythical
    }
    public static class EquipmentQualityEx
    {
        public static Color GetLockColor(this EquipmentQuality quality)
        {
            return quality switch
            {
                EquipmentQuality.Good => new Color(0, 125f / 255, 13f / 255),
                EquipmentQuality.Excellent => new Color(0, 40f / 255, 113f / 255),
                EquipmentQuality.Epic => new Color(60f / 255, 2f / 255, 67f / 255),
                EquipmentQuality.Mythical => new Color(97f / 255, 0, 13f / 255),
                _ => Color.white
            };
        }
        //物品框背景图颜色
        public static Color GetBgColor(this EquipmentQuality quality)
        {
            return quality switch
            {
                EquipmentQuality.Normal => new Color(151f / 255, 150f / 255, 150f / 255),
                EquipmentQuality.Good => new Color(13f / 255, 128f / 255, 24f / 255),
                EquipmentQuality.Excellent => new Color(57f / 255, 98f / 255, 149f / 255),
                EquipmentQuality.Epic => new Color(123f / 255, 33f / 255, 112f / 255),
                EquipmentQuality.Mythical => new Color(103f / 255, 0, 0),
                _ => Color.white
            };
        }
        //物品框边框颜色
        public static Color GetFrameColor(this EquipmentQuality quality)
        {
            return quality switch
            {
                EquipmentQuality.Normal => new Color(228f / 255, 233f / 255, 241f / 255),
                EquipmentQuality.Good => new Color(52f / 255, 202f / 255, 27f / 255),
                EquipmentQuality.Excellent => new Color(56f / 255, 131f / 255, 1),
                EquipmentQuality.Epic => new Color(1, 0, 1),
                EquipmentQuality.Mythical => new Color(1, 41f / 255, 41f / 255),
                _ => Color.white
            };
        }
        //文字描述字体颜色
        public static Color GetTextColor(this EquipmentQuality quality)
        {
            return quality switch
            {
                EquipmentQuality.Normal => new Color(228f / 255, 233f / 255, 241f / 255),
                EquipmentQuality.Good => new Color(30f / 255, 205f / 255, 0),
                EquipmentQuality.Excellent => new Color(26f / 255, 104f / 255, 234f / 255),
                EquipmentQuality.Epic => new Color(246f / 255, 37f / 255, 1),
                EquipmentQuality.Mythical => new Color(1, 0, 0),
                _ => Color.white
            };
        }
        public static string GetName(this EquipmentQuality quality)
        {
            return quality switch
            {
                EquipmentQuality.Normal => "普通",
                EquipmentQuality.Good => "精良",
                EquipmentQuality.Excellent => "优秀",
                EquipmentQuality.Epic => "史诗",
                EquipmentQuality.Legend => "传说",
                EquipmentQuality.Mythical => "神话",
                _ => "普通"
            };
        }

        public static string GetEquipName(this EquipQuality quality)
        {
            return quality switch
            {
                EquipQuality.Normal => "普通",
                EquipQuality.Good => "精良",
                EquipQuality.Excellent => "优秀",
                EquipQuality.Epic => "史诗",
                EquipQuality.Legend => "传说",
                EquipQuality.Mythical => "神话",
                _ => "普通"
            };
        }
    }
}