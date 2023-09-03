using System;
using System.Collections.Generic;
using System.Linq;

[Config]
public partial class GlobalConfig : BaseConfig
{
    private Dictionary<string, GlobalConfigItem> dict = new Dictionary<string, GlobalConfigItem>();
    private List<GlobalConfigItem> list = new List<GlobalConfigItem>();
	
    public override void Init()
    {
        dict = LoadConfig<Dictionary<string, GlobalConfigItem>>(typeof(GlobalConfig));
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