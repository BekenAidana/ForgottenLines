using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements.Experimental;
public class RoundPeople : Drawing
{
    [SerializeField] private Light2D globalLight;
    protected override void BehaviourOfActivatedObject()
    {
        base.BehaviourOfActivatedObject();
        StartCoroutine(AdjustScaleAndLight(1,0,3));
    }

    private IEnumerator AdjustScaleAndLight(float fromValue, float toValue, float duration)
    {
        LevelManager.instance.player.GetComponent<PlayerController>().GameEnd();
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float value =  Mathf.Lerp(fromValue, toValue, timer / duration);
            transform.localScale = new Vector3(value, value, value);
            globalLight.intensity = value;
            yield return null;
        }
        globalLight.intensity = toValue;
        GameManager.instance.NewGame();
    }
}
