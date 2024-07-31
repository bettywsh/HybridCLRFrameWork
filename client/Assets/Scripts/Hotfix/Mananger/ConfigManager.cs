using Cysharp.Threading.Tasks;
using UnityEngine;
using Luban;
using SimpleJSON;

public class ConfigManager : Singleton<ConfigManager>
{
    public cfg.Tables Tables;
    public override async UniTask Init()
    {
        await base.Init();
        var tablesCtor = typeof(cfg.Tables).GetConstructors()[0];
        var loaderReturnType = tablesCtor.GetParameters()[0].ParameterType.GetGenericArguments()[1];
        // 根据cfg.Tables的构造函数的Loader的返回值类型决定使用json还是ByteBuf Loader
        System.Delegate loader = loaderReturnType == typeof(ByteBuf) ?
            new System.Func<string, ByteBuf>(LoadByteBuf)
            : (System.Delegate)new System.Func<string, JSONNode>(LoadJson);
        Tables = (cfg.Tables)tablesCtor.Invoke(new object[] { loader });
    }

    private JSONNode LoadJson(string file)
    {
        TextAsset ta = ResManager.Instance.LoadAsset<TextAsset>($"Assets/App/Config/{file}.json");
        return JSON.Parse(ta.text);
    }

    private ByteBuf LoadByteBuf(string file)
    {
        TextAsset ta = ResManager.Instance.LoadAsset<TextAsset>($"Assets/App/Config/{file}.bytes");
        return new ByteBuf(ta.bytes);
    }

    public override void Dispose()
    {        
        base.Dispose();
    }
}

