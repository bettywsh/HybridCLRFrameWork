//---------------------------------
//此文件由工具自动生成,请勿手动修改
//---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

[Config]
public partial class SoundsConfig : BaseConfig
{
    private Dictionary<string, SoundsConfigItem> dict = new Dictionary<string, SoundsConfigItem>();
    private List<SoundsConfigItem> list = new List<SoundsConfigItem>();
	
    public override async UniTaskVoid InitUniTask()
    {
        dict = await LoadConfig<Dictionary<string, SoundsConfigItem>>(typeof(SoundsConfig));
		list = dict.Select(x => x.Value).ToList();
    }
	
    public SoundsConfigItem GetById(string id)
    {
		SoundsConfigItem item = null;	
        dict.TryGetValue(id, out item);
        if (item == null)
        {
            throw new Exception($"Not Find Config,ConfigName:{nameof (SoundsConfig)},ConfigId:{id}");
        }
        return item;
    }
	
	public List<SoundsConfigItem> GetAll()
	{
		return list;
	}
}

public partial class SoundsConfigItem
{
	public string Id;
	public string FileName;

}