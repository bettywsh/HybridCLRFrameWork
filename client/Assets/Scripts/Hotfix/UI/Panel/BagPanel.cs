using cfg;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : PanelBase
{
    List<cfg.Horse> listHorseConfig;

    public override async UniTask OnOpen()
    {
        await base.OnOpen();
        listHorseConfig = ConfigManager.Instance.Tables.HorseConfig.DataList;
        referenceCollector.Get("listListView").listValue.OnItemRender = OnItemRender;
        referenceCollector.Get("listListView").listValue.TotalCount = listHorseConfig.Count;
    }

    void OnItemRender(int idx, CellBase cb)
    {
        BagCell cell = cb as BagCell;
        cell.SetData(ConfigManager.Instance.Tables.HorseConfig.Get(idx + 1));
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
