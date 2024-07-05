using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CellBase : MonoBehaviour
{
    public Dictionary<string, ReferenceData> referenceData;
    ReferenceCollector referenceCollector;
    ListView listView;

    public virtual void Init(ListView lv)
    {
        listView = lv;
    }

    public virtual void OnBindEvent()
    {
        referenceCollector = transform.GetComponent<ReferenceCollector>();

        EventHelper.RegisterUIEvent(this, referenceCollector);
    }
}
