using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : Drawing
{   
    protected override void BehaviourOfActivatedObject()
    {
        base.BehaviourOfActivatedObject();
        if(effectPrefab != null)
        {
            effectPrefab.SetActive(true);
        }
    }
    public override void DiactivateObject()
    {
        if(effectPrefab != null)
        {
            effectPrefab.SetActive(false);   
        }
        base.DiactivateObject();
    }
}
