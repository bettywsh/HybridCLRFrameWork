public abstract class EventBase { }

//框架使用
public class EventLoadingPanelProgress : EventBase { }

public class EventLoadingPanelComplete : EventBase { }

public class EventReConnectPanelClose : EventBase { }


//业务逻辑

public class EventShowWorldBanner : EventBase { }
public class EventRedPointRefresh : EventBase
{
    public string Name;
    public EventRedPointRefresh(string name)
    {
        Name = name;
    }
}
public class EventAliAuthSucc : EventBase {
    public string Code;
    public EventAliAuthSucc(string code)
    {
        Code = code;
    }
}
public class EventAliAuthFail : EventBase
{
    public string Code;
    public EventAliAuthFail(string code)
    {
        Code = code;
    }
}
public class EventAliPaySucc : EventBase
{
    public string Code;
    public EventAliPaySucc(string code)
    {
        Code = code;
    }
}
public class EventAliPayFail : EventBase
{
    public string Code;
    public EventAliPayFail(string code)
    {
        Code = code;
    }
}
public class EventTapTapLoginSucc : EventBase
{
    public string OpenId;
    public EventTapTapLoginSucc(string openId)
    {
        OpenId = openId;
    }
}
public class EventTapTapLoginFail : EventBase
{
    public string OpenId;
    public EventTapTapLoginFail(string openId)
    {
        OpenId = openId;
    }
}
public class EventTapTapLoginCancel : EventBase
{
    public string OpenId;
    public EventTapTapLoginCancel(string openId)
    {
        OpenId = openId;
    }
}