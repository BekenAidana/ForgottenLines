using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private GameObject correctPart;
    private bool isPartPlaced = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject == correctPart) && (other.GetComponent<Drawing>()!=null) && (!isPartPlaced))
        {
            PartPlaced(other.GetComponent<Drawing>(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject == correctPart) && (other.GetComponent<Drawing>()!=null) && (isPartPlaced))
        {
            PartPlaced(other.GetComponent<Drawing>(), false);
        }
    }

    void PartPlaced(Drawing figure, bool value)
    {
        isPartPlaced = value;
        figure.SetIsPlacedCorrectly(isPartPlaced, transform.position);
    }
}
