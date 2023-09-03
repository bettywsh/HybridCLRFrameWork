using System;
using System.Collections.Generic;
using System.Linq;

[Config]
public partial class HorseConfig : BaseConfig
{
    private Dictionary<string, HorseConfigItem> dict = new Dictionary<string, HorseConfigItem>();
    private List<HorseConfigItem> list = new List<HorseConfigItem>();
	
    public override void Init()
    {
        dict = LoadConfig<Dictionary<string, HorseConfigItem>>(typeof(HorseConfig));
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