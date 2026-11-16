using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshProHelper : MonoBehaviour
{
    public static string TextToSprite(string number)
    {
        //默认是+
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i].ToString() == "万")
            {
                sprite += $"<sprite=10>";
            }
            else if (number[i].ToString() == "亿")
            {
                sprite += $"<sprite=11>";
            }
            else if (number[i].ToString() == ".")
            {
                sprite += $"<sprite=12>";
            }
            else
            {
                sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            }
        }
        return sprite;
    }

    public static string HandBookCellTextToSprite(string number)
    {
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i].ToString() == "倍")
            {
                sprite += $"<sprite=10>";
            }
            else
            {
                sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            }
        }
        return sprite;
    }

    public static string TextDouToSprite(string number)
    {
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i].ToString() == ",")
            {
                sprite += $"<sprite=10>";
            }
            else
            {
                sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            }
        }
        return sprite;
    }

    public static string ZeroPayToSprite(string number)
    {
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i].ToString() == "x")
            {
                sprite += $"<sprite=10>";
            }
            else if (number[i].ToString() == "万")
            {
                sprite += $"<sprite=11>";
            }
            else
            {
                sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            }
        }
        return sprite;
    }

    public static string LevelToSprite(string number)
    {
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i].ToString() == "第")
            {
                sprite += $"<sprite=10>";
            }
            else if (number[i].ToString() == "关")
            {
                sprite += $"<sprite=11>";
            }
            else
            {
                sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            }
        }
        return sprite;
    }
    public static string VipToSprite(string number)
    {
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i].ToString() == "V")
            {
                sprite += $"<sprite=10>";
            }
            else if (number[i].ToString() == "I")
            {
                sprite += $"<sprite=11>";
            }
            else if (number[i].ToString() == "P")
            {
                sprite += $"<sprite=12>";
            }
            else
            {
                sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            }
        }
        return sprite;
    }
    public static string DragonVaultText(string number)
    {
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
            if (number[i].ToString() == "/")
            {
                sprite += $"<sprite=10>";
            }
            else
            {
                sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            }
        }
        return sprite;
    }
    public static string Text10ToSprite(string number)
    {
        string sprite = "";
        for (int i = 0; i < number.Length; i++)
        {
         sprite += $"<sprite={int.Parse(number[i].ToString())}>";
            
        }
        return sprite;
    }
}
