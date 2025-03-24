using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneEffect : Effect
{
    [SerializeField] Vector2 windDirection = new Vector2(1f,0);
    void OnEnable()
    {
        CheckObjectsInZone();
    }

    public void CheckObjectsInZone()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true; 
        GetComponent<Collider2D>().OverlapCollider(filter, colliders);

        foreach (Collider2D collider in colliders)
        {
            Cloud cloud = collider.GetComponent<Cloud>();
            if (cloud != null)
            {
                cloud.SetWindZone(createdFigure, true, windDirection);
            }
        }
    }

}
