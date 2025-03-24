using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Effect
{
    private float speed = 7f;

    void Start()
    {
        objRenderer.material.SetFloat("_Appearance", 1);
    }

    public override void DestroyObject()
    {
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return StartCoroutine(AdjustFinalAppearance(1f,0f,0.5f));
        Destroy(gameObject);
    }
    
    void Update()
    {
        transform.Translate(Vector3.right*Time.deltaTime*speed, Space.World);
    }
}
