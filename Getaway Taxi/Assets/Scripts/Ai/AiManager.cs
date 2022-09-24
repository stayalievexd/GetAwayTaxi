using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{
    [SerializeField] List<Transform> routePoints = new List<Transform>();

    public Transform getNewPoint(Transform lastPos)
    {
        Transform newReturn = null;
        if(lastPos == null)
        {
            newReturn = routePoints[Random.Range(0,routePoints.Count-1)];
        }
        else
        {
            while(newReturn == null || newReturn == lastPos)
            {
                newReturn = routePoints[Random.Range(0,routePoints.Count-1)];
            }
        }

        return newReturn;
    }
}
