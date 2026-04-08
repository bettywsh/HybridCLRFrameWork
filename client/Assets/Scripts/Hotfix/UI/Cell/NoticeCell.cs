using Game;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoticeCell : MonoBehaviour
{
    
    public int id;
    public Toggle toggle;
    public TextMeshProUGUI utext;
    public TextMeshProUGUI stext;

    private void Awake()
    {
        toggle.onValueChanged.AddListener((on) =>
        {
            if (on)
            {
                EventManager.Instance.MessageNotify(MessageConst.Msg_NoticeTitleClick, id);
            }
        });
    }

    public void SetText(string str)
    {
        utext.text = str;
        stext.text= str;
    }
   
    
}