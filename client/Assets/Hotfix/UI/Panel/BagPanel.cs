using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : BasePanel
{
	public BagPanelView view;
    List<HorseConfigItem> listHorseConfig;

    public override void OnBindEvent()
    {
        view = transform.GetComponent<BagPanelView>();
        base.OnBindEvent();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        listHorseConfig = ConfigManager.Instance.LoadConfig<HorseConfig>().GetAll();
        view.lsv_ListView.SetItemRender(this, OnItemRender);
        view.lsv_ListView.TotalCount = listHorseConfig.Count;
    }

    void OnItemRender(int idx, Transform tf)
    {
        tf.GetComponent<BagCell>().SetData(idx, listHorseConfig);
    }

    void Click_btn_Mask()
    {
        UIManager.Instance.Close<BagPanel>();
    }


    public override void OnClose()
    {
        base.OnClose();
    }
}
