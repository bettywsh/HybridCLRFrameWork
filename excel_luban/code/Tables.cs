
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
public partial class Tables
{
    public itemConfig ItemConfig {get; }

    public Tables(System.Func<string, JSONNode> loader)
    {
        ItemConfig = new itemConfig(loader("itemconfig"));
        ResolveRef();
    }
    
    private void ResolveRef()
    {
        ItemConfig.ResolveRef(this);
    }
}

}
