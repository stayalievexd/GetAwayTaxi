using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollider : MonoBehaviour
{

    [Header("Crash settings")]
    
    [Tooltip("Added collision force multiplier")]
    [SerializeField] private Vector2 forceMultiplier = new Vector2(10,20);

    [Tooltip("BackForce on player car")]
    [SerializeField] private Vector2 backwardsForce = new Vector2(10,20);

    [Header("Scripts/Components")]
    
    [Tooltip("Car stats script")]
    [SerializeField] private CarStats statsScript;

    [Tooltip("Car movement script")]
    [SerializeField] private Car carMovementScript;

    [Tooltip("RigidBody")]
    [SerializeField] private Rigidbody carRb;

    private void OnCollisionEnter(Collision other)
    {
        GameObject rootObject = other.transform.root.gameObject;
        if(rootObject.layer == LayerMask.NameToLayer("Ai"))
        {
            bool collided = rootObject.GetComponent<StateManager>().setCrashed(transform.forward * statsScript.getSpeed() * Random.Range(forceMultiplier.x,forceMultiplier.y));
            
            if(!collided)
            {
                carRb.AddForce(transform.forward * -statsScript.getSpeed() * Random.Range(backwardsForce.x,backwardsForce.y));
            }
        }

        carMovementScript.collision(statsScript.getSpeed());
    }
}
