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
    public string Name;
    public OnSliderChangedAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class OnToggleChangedAttribute : Attribute
{
    public string Name;
    public OnToggleChangedAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class OnClickAttribute : Attribute
{
    public string Name;
    public OnClickAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class OnMessageAttribute : Attribute
{
    public string Name;
    public OnMessageAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class OnNetAttribute : Attribute
{
    public int Id;
    public OnNetAttribute(int id)
    {
        Id = id;
    }
}