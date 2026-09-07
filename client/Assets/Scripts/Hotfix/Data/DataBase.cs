using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase 
{
    public virtual void Init()
    {
        EventHelper.RegisterAllEvent(this);
    }

    public virtual void Reset()
    {

    }

    public virtual void Dispose()
    {
        // EventHelper.UnRegisterAllEvent(this);
    }
}
