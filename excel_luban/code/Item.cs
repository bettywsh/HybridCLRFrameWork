
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public sealed partial class Item : Luban.BeanBase
{
    public Item(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["name"].IsString) { throw new SerializationException(); }  Name = _buf["name"]; }
        { if(!_buf["desc"].IsString) { throw new SerializationException(); }  Desc = _buf["desc"]; }
    }

    public static Item DeserializeItem(JSONNode _buf)
    {
        return new Item(_buf);
    }

    /// <summary>
    /// 这是id
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 名字
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 描述
    /// </summary>
    public readonly string Desc;
   
    public const int __ID__ = 2289459;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "desc:" + Desc + ","
        + "}";
    }
}

}
