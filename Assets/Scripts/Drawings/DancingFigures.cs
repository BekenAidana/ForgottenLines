using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingFigures : Drawing
{
    private Effect effect;
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if(other.GetComponent<FireEffect>()!=null && isCanInteract)
        {
            effect = other.GetComponent<FireEffect>();
            if(effect.createdFigure != this.gameObject)
            {
                levelManager.AddToChain(this, effect.createdFigure.GetComponent<Drawing>());
                effect.DestroyObject();
            }
        }
    }
}
