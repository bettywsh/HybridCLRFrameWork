using System;

[AttributeUsage(AttributeTargets.Class)]
public class BaseAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class EventAttribute : BaseAttribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class PanelAttribute : BaseAttribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class SubPanelAttribute : BaseAttribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class CellAttribute : BaseAttribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class SceneAttribute : BaseAttribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class ConfigAttribute : BaseAttribute
{

}

[AttributeUsage(AttributeTargets.Class)]
public class DataAttribute : BaseAttribute
{ 

}

[AttributeUsage(AttributeTargets.Method)]
public class OnSliderChangedAttribute : Attribute
{
    public readonly string Name;
    public OnSliderChangedAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class OnToggleChangedAttribute : Attribute
{
    public readonly string Name;
    public OnToggleChangedAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class OnClickAttribute : Attribute
{
    public readonly string Name;
    public OnClickAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class OnMessageAttribute : Attribute
{
    public readonly int Name;
    public OnMessageAttribute(int name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class OnTimerAttribute : Attribute
{
    public readonly int Name;
    public OnTimerAttribute(int name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class OnNetAttribute : Attribute
{
    public readonly int Id;
    public OnNetAttribute(int id)
    {
        Id = id;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class RedPointAttribute : Attribute
{
    public readonly int Id;
    public RedPointAttribute(int id)
    {
        Id = id;
    }
}