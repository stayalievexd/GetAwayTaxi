using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtCheck : MonoBehaviour
{   
    /*
        Check how many meters the car has moved in the last few seconds 
        check how many cop cars are close to the player
        
    */

    [Header("Caught Settings")]
    
    [Tooltip("Distance checked for police cars")]
    [SerializeField] private float coughtDistance = 100;

    [Tooltip("Distance checked for police cars")]
    [SerializeField] private LayerMask checkLayers;

    void FixedUpdate()
    {
        // checkCopRadius();
    }

    private void checkCopRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coughtDistance,checkLayers);
        // foreach (var hitCollider in hitColliders)
        // {
        //     // Debug.Log(hitColliders.Length);
        // }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, coughtDistance);
    }
}
