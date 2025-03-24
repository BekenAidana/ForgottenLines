using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireEffect : Effect
{
    [SerializeField] private Light2D globalLight;
    private Light2D innerLight;
    protected override void Awake()
    {
        base.Awake();
        innerLight = GetComponent<Light2D>();
    }

    public void DecreaseFireLight()
    {
        StartCoroutine(DecreaseFireLightCoroutine());
    }

    public void IncreaseFireLight()
    {
        StartCoroutine(IncreaseFireLightCoroutine());
    }

    private IEnumerator IncreaseFireLightCoroutine()
    {
        yield return StartCoroutine(AdjustLightIntensity(innerLight, innerLight.intensity, 7, 3));
        if (globalLight !=null) yield return StartCoroutine(AdjustLightIntensity(globalLight,globalLight.intensity, 1, 3));
        yield return null;
    }

    private IEnumerator DecreaseFireLightCoroutine()
    {
        yield return StartCoroutine(AdjustLightIntensity(innerLight, innerLight.intensity, 0, 3));
        if (globalLight !=null) yield return StartCoroutine(AdjustLightIntensity(globalLight,globalLight.intensity, 0, 3));
        yield return null;
    }

    private IEnumerator AdjustLightIntensity(Light2D light, float fromValue, float toValue, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newIntensity = Mathf.Lerp(fromValue, toValue, timer / duration);
            light.intensity = newIntensity;
            yield return null;
        }
        light.intensity = toValue;
    }
}
