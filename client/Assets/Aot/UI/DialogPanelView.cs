//---------------------------------
//此文件由工具自动生成,请勿手动修改
//---------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class DialogPanel 
{
	public Button btn_Mask;
	public CanvasGroup cng_Msg;
	public TextMeshProUGUI txt_Msg;
	public Button btn_Cancel;
	public TextMeshProUGUI txt_Cancel;
	public Button btn_Ok;
	public TextMeshProUGUI txt_Ok;


   public override void Awake()
   {
        btn_Mask.onClick.AddListener(OnClick_btn_Cancel);
        btn_Ok.onClick.AddListener(OnClick_btn_Ok);
        btn_Cancel.onClick.AddListener(OnClick_btn_Cancel);
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
