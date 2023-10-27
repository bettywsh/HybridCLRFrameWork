using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : BasePanel
{
    List<HorseConfigItem> listHorseConfig;

    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        listHorseConfig = ConfigManager.Instance.LoadConfig<HorseConfig>().GetAll();
        referenceData["ListView"].listValue.SetItemRender(this, OnItemRender);
        referenceData["ListView"].listValue.TotalCount = listHorseConfig.Count;
    }

    void OnItemRender(int idx, Transform tf)
    {
        tf.GetComponent<BagCell>().SetData(idx, listHorseConfig);
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
