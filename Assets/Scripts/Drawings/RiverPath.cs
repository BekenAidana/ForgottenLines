using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverPath : Drawing
{
    [SerializeField] private int riverPathIndex;
    [SerializeField] private int riverPathCount;
    [SerializeField] private float increment;
    [SerializeField] private RiverManager riverManager;
    [SerializeField] public bool isCorrectlyPlaced;
    private BoxCollider2D colliderObj;

    protected override void Start()
    {
        base.Start();
        colliderObj = GetComponent<BoxCollider2D>();
        increment = 1.0f/(float)riverPathCount;
        objRenderer.material.SetFloat("_TilingX", increment);
        objRenderer.material.SetFloat("_OffsetX", riverPathIndex*increment);
    }

    public override void SetIsPlacedCorrectly(bool value, Vector3 triggerPosition)
    {
        isCorrectlyPlaced = value;
        if(value)
        {
            transform.position=triggerPosition;
            colliderObj.isTrigger=true;
        }
        else{colliderObj.isTrigger=false;}

    }

    protected override void BehaviourOfActivatedObject()
    {
        base.BehaviourOfActivatedObject();
        if(isCorrectlyPlaced) riverManager.ActivateNextPath(this);
    }

    public void ActivatedByOrder(RiverPath causedRiverPath)
    {
        if(isCorrectlyPlaced) levelManager.AddToChain(this, causedRiverPath);
    }

    public void SetFinalApperance(float value)
    {
        objRenderer.material.SetFloat("_FinalAppearance", 0);
    }

    public bool GetActiveInChain()
    {
        return isActiveInChain;
    }
}
