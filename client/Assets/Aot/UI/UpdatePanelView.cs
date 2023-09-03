//---------------------------------
//此文件由工具自动生成,请勿手动修改
//---------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class UpdatePanel 
{
	public Image img_cur;
	public TextMeshProUGUI txt_Content;

   public override void Awake()
   {
        AotMessage.Instance.RegisterMessageHandler(AotMessageConst.Msg_UpdateFristCopy, Msg_UpdateFristCopy);
        AotMessage.Instance.RegisterMessageHandler(AotMessageConst.Msg_UpdateFristProgress, Msg_UpdateFristProgress);
        AotMessage.Instance.RegisterMessageHandler(AotMessageConst.Msg_UpdateCheckVersion, Msg_UpdateCheckVersion);
        AotMessage.Instance.RegisterMessageHandler(AotMessageConst.Msg_UpdateBigVersion, Msg_UpdateBigVersion);
        AotMessage.Instance.RegisterMessageHandler(AotMessageConst.Msg_UpdateSmallVersion, Msg_UpdateSmallVersion);
        AotMessage.Instance.RegisterMessageHandler(AotMessageConst.Msg_UpdateLostConnect, Msg_UpdateLostConnect);
    }
   
   public override void Start()
   {
       OnOpen();
   }
    public override void OnDestroy()
    {
        OnClose();
    }
}
