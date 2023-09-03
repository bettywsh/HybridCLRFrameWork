using System;
using System.Collections.Generic;
using System.Linq;

[Config]
public partial class ChineseTextConfig : BaseConfig
{
    private Dictionary<string, ChineseTextConfigItem> dict = new Dictionary<string, ChineseTextConfigItem>();
    private List<ChineseTextConfigItem> list = new List<ChineseTextConfigItem>();
	
    public override void Init()
    {
        dict = LoadConfig<Dictionary<string, ChineseTextConfigItem>>(typeof(ChineseTextConfig));
		list = dict.Select(x => x.Value).ToList();
    }
	
    public ChineseTextConfigItem GetById(string id)
    {
		ChineseTextConfigItem item = null;	
        dict.TryGetValue(id, out item);
        if (item == null)
        {
            throw new Exception($"Not Find Config,ConfigName:{nameof (ChineseTextConfig)},ConfigId:{id}");
        }
        return item;
    }
	
	public List<ChineseTextConfigItem> GetAll()
	{
		return list;
	}
}

public partial class ChineseTextConfigItem
{
	public string Id;
	public string Value;

}