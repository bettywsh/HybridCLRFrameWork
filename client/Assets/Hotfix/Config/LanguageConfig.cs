//---------------------------------
//此文件由工具自动生成,请勿手动修改
//---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

[Config]
public partial class LanguageConfig : BaseConfig
{
    private Dictionary<string, LanguageConfigItem> dict = new Dictionary<string, LanguageConfigItem>();
    private List<LanguageConfigItem> list = new List<LanguageConfigItem>();
	
    public override async UniTaskVoid InitUniTask()
    {
        dict = await LoadConfig<Dictionary<string, LanguageConfigItem>>(typeof(LanguageConfig));
		list = dict.Select(x => x.Value).ToList();
    }
	
    public LanguageConfigItem GetById(string id)
    {
		LanguageConfigItem item = null;	
        dict.TryGetValue(id, out item);
        if (item == null)
        {
            throw new Exception($"Not Find Config,ConfigName:{nameof (LanguageConfig)},ConfigId:{id}");
        }
        return item;
    }
	
	public List<LanguageConfigItem> GetAll()
	{
		return list;
	}
}

public partial class LanguageConfigItem
{
	public string Id;
	public string Chinese;

}