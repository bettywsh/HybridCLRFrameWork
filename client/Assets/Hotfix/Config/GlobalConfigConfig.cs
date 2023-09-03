using System;
using System.Collections.Generic;
using System.Linq;

[Config]
public partial class GlobalConfigConfig : BaseConfig
{
    private Dictionary<string, GlobalConfigConfigItem> dict = new Dictionary<string, GlobalConfigConfigItem>();
    private List<GlobalConfigConfigItem> list = new List<GlobalConfigConfigItem>();
	
    public override void Init()
    {
        dict = LoadConfig<Dictionary<string, GlobalConfigConfigItem>>(typeof(GlobalConfigConfig));
		list = dict.Select(x => x.Value).ToList();
    }
	
    public GlobalConfigConfigItem GetById(string id)
    {
		GlobalConfigConfigItem item = null;	
        dict.TryGetValue(id, out item);
        if (item == null)
        {
            throw new Exception($"Not Find Config,ConfigName:{nameof (GlobalConfigConfig)},ConfigId:{id}");
        }
        return item;
    }
	
	public List<GlobalConfigConfigItem> GetAll()
	{
		return list;
	}
}

public partial class GlobalConfigConfigItem
{
	public string Id;
	public string Value;

}