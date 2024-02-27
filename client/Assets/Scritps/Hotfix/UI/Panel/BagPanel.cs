using cfg;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : BasePanel
{
    List<cfg.Horse> listHorseConfig;

    public override async UniTask OnBindEvent()
    {
        base.OnBindEvent();
    }

    public override async UniTask OnOpen()
    {
        base.OnOpen();
        listHorseConfig = ConfigManager.Instance.Tables.HorseConfig.DataList;
        referenceData["ListView"].listValue.SetItemRender(this, OnItemRender);
        referenceData["ListView"].listValue.TotalCount = listHorseConfig.Count;
    }

    void OnItemRender(int idx, Transform tf)
    {
        tf.GetComponent<BagCell>().SetData(idx, ConfigManager.Instance.Tables.HorseConfig.Get(idx + 1));
    }

    void OnClick_Mask()
    {
        UIManager.Instance.Close<BagPanel>();
    }


    public override void OnClose()
    {
        base.OnClose();
    }
}
