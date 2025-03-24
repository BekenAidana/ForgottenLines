using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Drawing
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.GetComponent<RainDropEffect>()!=null)
        {
            RainDropEffect drop = other.GetComponent<RainDropEffect>();
            levelManager.AddToChain(this, drop.createdFigure.GetComponent<Drawing>());
            drop.Grounded();
        }
    }
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
            FlowerEffect flower = effectPrefab.GetComponent<FlowerEffect>();
            if(flower != null)
            {
                flower.DestroyObject();   
            }
        }
        base.DiactivateObject();
    }
}
