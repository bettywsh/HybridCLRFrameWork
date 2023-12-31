//---------------------------------
//此文件由工具自动生成,请勿手动修改
//---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

[Config]
public partial class HorseConfig : BaseConfig
{
    private Dictionary<string, HorseConfigItem> dict = new Dictionary<string, HorseConfigItem>();
    private List<HorseConfigItem> list = new List<HorseConfigItem>();
	
    public override async UniTaskVoid InitUniTask()
    {
        dict = await LoadConfig<Dictionary<string, HorseConfigItem>>(typeof(HorseConfig));
		list = dict.Select(x => x.Value).ToList();
    }
	
    public HorseConfigItem GetById(string id)
    {
		HorseConfigItem item = null;	
        dict.TryGetValue(id, out item);
        if (item == null)
        {
            throw new Exception($"Not Find Config,ConfigName:{nameof (HorseConfig)},ConfigId:{id}");
        }
        return item;
    }
	
	public List<HorseConfigItem> GetAll()
	{
		return list;
	}
}

public partial class HorseConfigItem
{
	public int Id;
	public string Icon;
	public string Name;

}