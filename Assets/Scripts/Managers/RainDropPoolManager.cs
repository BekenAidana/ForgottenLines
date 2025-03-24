using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDropPoolManager : MonoBehaviour
{
    public int poolSize = 10;
    [SerializeField] private Queue<GameObject> pool = new Queue<GameObject>();
    [SerializeField] private GameObject rainDropPrefab;

    public void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject rainDrop = Instantiate(rainDropPrefab);
            // rainDrop.transform.parent = transform;
            rainDrop.SetActive(false);
            pool.Enqueue(rainDrop);
        }
    }
    public GameObject GetRainDrop()
    {
        
        if (pool.Count > 0)
        {
            GameObject rainDrop = pool.Dequeue();
            rainDrop.SetActive(true);
            return rainDrop;
        }
        else
        {
            GameObject rainDrop = Instantiate(rainDropPrefab);
            // rainDrop.transform.parent = transform;
            rainDrop.SetActive(true);
            return rainDrop;
        }
    }
    public void ReturnRainDrop(GameObject rainDrop)
    {
        Debug.Log("Возвращаем каплю в пул: " + rainDrop.name);
        
        rainDrop.SetActive(false);
        pool.Enqueue(rainDrop);

        Debug.Log("Количество объектов в пуле после возврата: " + pool.Count);
    }
}