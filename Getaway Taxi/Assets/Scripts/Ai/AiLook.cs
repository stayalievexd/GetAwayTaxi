using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiLook : MonoBehaviour
{
    [Header("States")]
    
    [Tooltip("Distance checked for the player car")]
    [SerializeField] private ChaseState chaseState;
    [SerializeField] private AmbushState ambushState;
    [SerializeField] private PatrolState patrolState;
    
    [Header("check Settings")]

    [Tooltip("Distance checked for the player car")]
    [SerializeField] private float viewDistance = 300;
    
    [Tooltip("Distance checked for the player car")]
    [SerializeField] private float triggerDistance = 40;

    [Tooltip("Time Player can not be inview ")]
    [SerializeField] private float maxNoViewTime = 50;

    [Tooltip("Distance checked for police cars")]
    [SerializeField] private LayerMask checkLayers;
    [SerializeField] private LayerMask viewLayers;

    [Header("Private data")]
    private bool inTrigger = false;
    private bool inView = false;
    private float viewTime = 0;
    private Transform player;

    void FixedUpdate()
    {
        if(!inTrigger)
        {
            checkCopRadius();
        }
        else
        {
            checkView();
        }
    }

    private void checkCopRadius()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, triggerDistance,checkLayers);
        if(hitColliders.Length > 0)
        {
            setIntrigger(hitColliders[0].transform);
        }
    }

    private void setIntrigger(Transform hitTransform)
    {
        player = hitTransform.root.transform;
        inTrigger = true;
        viewTime = 0;
    }

    private void checkView()
    {
        if(player)
        {
            if(Vector3.Distance(transform.position, player.position) < viewDistance )
            {
                RaycastHit hit;
                Ray ray = new Ray(transform.position, (player.position - transform.position));
                if(Physics.Raycast(ray, out hit, viewDistance, viewLayers))
                {
                    setViewTime(hit.transform.root.gameObject.tag == "Player");
                }
                else
                {
                    setViewTime(false);
                   // Debug.DrawRay(ray.origin, ray.direction * viewDistance, Color.green);
                }
            }
        }
    }

    private void setViewTime(bool see)
    {
        if(see)
        {
            if(!inView)
            {   
                setInview(true);//in view
            }
            if(viewTime < maxNoViewTime)
            {
                viewTime ++;
            }
        }
        else
        {
            if(viewTime > 0)
            {
                viewTime --;
            }
            else
            {
                if(inView)
                {
                    setInview(false);//out of view
                }
            }
        }
    }

    private void setInview(bool active)
    {
        inView = active;
        if(active)//in view
        {
            viewTime = maxNoViewTime;
            chaseState.setStartState(player);
        }
        else//ouyt of view
        {
            chaseState.outOfView();
        }
        setStates();
    }

    /*Draws sphere around the player for visualizing the range of the trigger*/
    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawSphere(transform.position, triggerDistance);
    // }

    private void setStates()//sets the state values for the state machine
    {
        chaseState.canSeePlayer = inView;
        ambushState.canSeePlayer = inView;
        patrolState.canSeePlayer = inView;
    }
}
