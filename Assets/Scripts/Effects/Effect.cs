using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    protected Renderer objRenderer;
    public GameObject createdFigure;

    protected virtual void Awake()
    {
        objRenderer = GetComponent<Renderer>();
    }

    public virtual void DestroyObject()
    {
    
    }

    protected IEnumerator AdjustFinalAppearance(float fromValue, float toValue, float duration)
    {
        duration = duration * Mathf.Abs(fromValue - toValue);
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAppearance = Mathf.Lerp(fromValue, toValue, timer / duration);
            objRenderer.material.SetFloat("_FinalAppearance", newAppearance);
            yield return null;
        }
        objRenderer.material.SetFloat("_FinalAppearance", toValue);
    }
}
