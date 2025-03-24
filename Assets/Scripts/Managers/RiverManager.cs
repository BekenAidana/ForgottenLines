using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RiverManager : MonoBehaviour
{
    [SerializeField] List<RiverPath> riverPathsList;
    [SerializeField] GameObject riverObj;

    public void ActivateNextPath(RiverPath riverpath)
    {
        int indexCurrentRiverPath = riverPathsList.IndexOf(riverpath);
        if(indexCurrentRiverPath>-1 && indexCurrentRiverPath<riverPathsList.Count-1)
        {
            riverPathsList[indexCurrentRiverPath+1].ActivatedByOrder(riverpath);
        }

        bool allPlacedCorrectlyInChain = riverPathsList.All(river => river.isCorrectlyPlaced && river.GetActiveInChain());

        if(indexCurrentRiverPath==riverPathsList.Count-1 && allPlacedCorrectlyInChain)
        {
            foreach (var river in riverPathsList)
            {
                river.SetFinalApperance(0);
            }
            riverObj.SetActive(true);
            
        }
    }

}
