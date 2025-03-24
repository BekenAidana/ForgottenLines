using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Cloud : Drawing
{
    public RainDropPoolManager poolManager;
    public float rainInterval = 0.1f;
    public bool isRaining=false;
    private bool isWindZone=false;
    private float windDirection=1;

    protected override void BehaviourOfActivatedObject()
    {
        base.BehaviourOfActivatedObject();
        StartRain();
    }

    private void StartRain()
    {
        if (!isRaining && isWindZone)
        {
            isRaining = true;
            StartCoroutine(RainCoroutine());
        }
    }

    private IEnumerator RainCoroutine()
    {
        int rainDropped =0;
        while (rainDropped<10)
        {
            GameObject rainDrop = poolManager.GetRainDrop();
            if(rainDrop!=null)
            {
                rainDrop.transform.position = transform.position;
                rainDrop.GetComponent<RainDropEffect>().Initialize(poolManager, this.gameObject, windDirection==1);
                rainDrop.GetComponent<RainDropEffect>().StartFalling();
            }
            rainDropped++;
            yield return new WaitForSeconds(rainInterval);
        }
    }

    public override void DiactivateObject()
    {
        isRaining = false;
        isWindZone = false;
        base.DiactivateObject();
    }

    public void SetWindZone(GameObject createdFigure, bool windZone, Vector2 windDirectionV)
    {
        levelManager.AddToChain(this, createdFigure.GetComponent<Drawing>());
        isWindZone = windZone;
        windDirection = windDirectionV.x;
        StartMove();
    }

    public void StartMove()
    {
        StartCoroutine(MoveCoroutine(168f)); 
    }
    private IEnumerator MoveCoroutine(float moveToPositionX)
    {
        yield return new WaitForSeconds(3);
        float duration = 6f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(moveToPositionX, transform.position.y, transform.position.z);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

}
