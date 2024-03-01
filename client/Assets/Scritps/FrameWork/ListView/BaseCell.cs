using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BaseCell : MonoBehaviour
{
    public Dictionary<string, ReferenceData> referenceData;
    public Transform transform;
    ReferenceCollector referenceCollector;
    ListView listView;

    public virtual void Init(ListView lv)
    {
        listView = lv;
    }

    public virtual void OnBindEvent()
    {
        referenceCollector = transform.GetComponent<ReferenceCollector>();
        referenceData = referenceCollector.data.ToDictionary(x => x.name, x => x.referenceData);

        EventHelper.RegisterUIEvent(this, referenceData);
    }
}
