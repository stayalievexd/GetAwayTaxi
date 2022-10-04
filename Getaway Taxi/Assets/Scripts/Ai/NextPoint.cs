using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPoint : MonoBehaviour
{
   [SerializeField] private Transform[] nextPoints;
   [SerializeField] private Color drawLine = Color.blue;

    public Transform nextPoint()
    {
        return nextPoints[Random.Range(0,nextPoints.Length)];
    }

    void OnDrawGizmosSelected()
    {
        if(nextPoints.Length > 0)
        {
            Gizmos.color = drawLine;
            for(int i=0; i<nextPoints.Length; i++)
            {
                Gizmos.DrawLine(transform.position,nextPoints[i].position);
            }
        }
    }
}
