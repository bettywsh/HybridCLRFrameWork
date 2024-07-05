using System;

[AttributeUsage(AttributeTargets.Class)]
public class BaseAttribute : Attribute
{
}

public class EventAttribute : BaseAttribute
{

}

public class UIAttribute : BaseAttribute
{

}

public class SceneAttribute : BaseAttribute
{

}

public class ConfigAttribute : BaseAttribute
{

}


public class DataAttribute : BaseAttribute
{ 

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