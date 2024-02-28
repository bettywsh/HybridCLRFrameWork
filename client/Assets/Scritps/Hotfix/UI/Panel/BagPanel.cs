using cfg;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : BasePanel
{
    List<cfg.Horse> listHorseConfig;

    public override async UniTask OnOpen()
    {
        await base.OnOpen();
        listHorseConfig = ConfigManager.Instance.Tables.HorseConfig.DataList;
        referenceData["listListView"].listValue.OnItemRender = OnItemRender;
        referenceData["listListView"].listValue.TotalCount = listHorseConfig.Count;
    }

    void OnItemRender(int idx, Transform tf)
    {
        tf.GetComponent<BagCell>().OnOpen(ConfigManager.Instance.Tables.HorseConfig.Get(idx + 1));
    }

    [OnClick("btnMask")]
    public void OnClick_Mask()
    {
        this.Close();
    }


    public override void OnClose()
    {
        base.OnClose();
    }
}
