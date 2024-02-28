

using System;

public class ConfigAttribute : Attribute
{

}


public class DataAttribute : Attribute
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