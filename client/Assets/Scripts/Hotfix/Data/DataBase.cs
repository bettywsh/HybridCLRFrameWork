using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase 
{
    public virtual void Init()
    {
        EventHelper.RegisterNetEvent(this);
        EventHelper.RegisterTimerEvent(this);
        EventHelper.RegisterMessageEvent(this);
    }

    public virtual void Reset()
    {

    }
}
