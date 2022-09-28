using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    /*
        Needs to do sphere check 
        Then needs to raycast every half second to the player if it can be seen 
        If hit set last seen count again to default
        when missed lower last seen counter
        when not seen anymore move to last seen hit position still check for sphere radius
    */

    [Header("AI Components")]

    [Tooltip("Nav Mesh Agent of the Ai")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Private Data")]
    private CarBodyScript bodyScript;
    private AiCarInformation carInfo;
    private Transform target;

    [Header("State machine values")]
    public RamState ramState;
    public PatrolState patrolState;
    public bool isInAttackRage;
    public bool canSeePlayer;

    public void setStart(AiCarInformation info,CarBodyScript newBodyScript)//start function gets called from the controller of the car
    {
        carInfo = info;
        bodyScript = newBodyScript;
    }

    public override State runThisState()
    {
        if(canSeePlayer)
        {
            if(isInAttackRage)
            {
                return ramState;
            }
            else
            {
                chase();
                return this;
            }
        }
        else
        {
            stopChase();

            return patrolState;
        }
    }

    private void chase()
    {
        agent.SetDestination(target.position);
    }

    private void stopChase()
    {
        bodyScript.setChase(false);//turns on chase lights
        patrolState.setPoint();//sets new patrol point
    }

    public void setStartState(Transform newTarget)
    {
        agent.speed = Random.Range(carInfo.chaseSpeed.x,carInfo.chaseSpeed.y);//sets chase speed
        target = newTarget;
      
        bodyScript.setChase(true);//turns on chase lights
    }
}
