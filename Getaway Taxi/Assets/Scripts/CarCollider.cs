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
    
    [Header("Components")]
    
    [Tooltip("RigidBody")]
    [SerializeField] private Rigidbody carRb;

    [Header("Private scripts")]
    private CarStats statsScript;
    private Car carMovementScript;

    public void setStartData(Car newMovement,CarStats newStats)
    {
        carMovementScript = newMovement;
        statsScript = newStats;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!Values.pauzed)
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
}
