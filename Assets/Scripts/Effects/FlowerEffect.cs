using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerEffect : Effect
{
    public override void DestroyObject()
    {
        StartCoroutine(DestroyCoroutine());
    }
    private IEnumerator DestroyCoroutine()
    {
        yield return StartCoroutine(AdjustFinalAppearance(1f,0f,0.5f));
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Debug.Log("Moi cveto4ek otkl");
    }
}
