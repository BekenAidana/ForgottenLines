using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fire : Drawing
{
    protected override void BehaviourOfActivatedObject()
    {
        base.BehaviourOfActivatedObject();
        effectPrefab.GetComponent<FireEffect>().IncreaseFireLight();
    }

    public override void DiactivateObject()
    {
        base.DiactivateObject();
        effectPrefab.GetComponent<FireEffect>().DecreaseFireLight();
    }
    
}
