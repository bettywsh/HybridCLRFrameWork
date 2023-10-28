//---------------------------------
//此文件由工具自动生成,请勿手动修改
//---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

[Config]
public partial class GlobalConfig : BaseConfig
{
    private Dictionary<string, GlobalConfigItem> dict = new Dictionary<string, GlobalConfigItem>();
    private List<GlobalConfigItem> list = new List<GlobalConfigItem>();
	
    public override async UniTaskVoid InitUniTask()
    {
        dict = await LoadConfig<Dictionary<string, GlobalConfigItem>>(typeof(GlobalConfig));
		list = dict.Select(x => x.Value).ToList();
    }
	
    public GlobalConfigItem GetById(string id)
    {
		GlobalConfigItem item = null;	
        dict.TryGetValue(id, out item);
        if (item == null)
        {
            throw new Exception($"Not Find Config,ConfigName:{nameof (GlobalConfig)},ConfigId:{id}");
        }
        return item;
    }
	
	public List<GlobalConfigItem> GetAll()
	{
		return list;
	}
}

public partial class GlobalConfigItem
{
	public string Id;
	public string Value;

}