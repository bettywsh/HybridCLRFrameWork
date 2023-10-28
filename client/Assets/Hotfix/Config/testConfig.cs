//---------------------------------
//此文件由工具自动生成,请勿手动修改
//---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

[Config]
public partial class testConfig : BaseConfig
{
    private Dictionary<string, testConfigItem> dict = new Dictionary<string, testConfigItem>();
    private List<testConfigItem> list = new List<testConfigItem>();
	
    public override async UniTaskVoid InitUniTask()
    {
        dict = await LoadConfig<Dictionary<string, testConfigItem>>(typeof(testConfig));
		list = dict.Select(x => x.Value).ToList();
    }
	
    public testConfigItem GetById(string id)
    {
		testConfigItem item = null;	
        dict.TryGetValue(id, out item);
        if (item == null)
        {
            throw new Exception($"Not Find Config,ConfigName:{nameof (testConfig)},ConfigId:{id}");
        }
        return item;
    }
	
	public List<testConfigItem> GetAll()
	{
		return list;
	}
}

public partial class testConfigItem
{
	public string Id;
	public string Value;
	public int xx;
	public bool xxx;

}