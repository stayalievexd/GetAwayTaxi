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
    private AiManager managerScript;
    private Transform target;

    [Header("State machine values")]
    public PatrolState patrolState;
    public bool canSeePlayer;

    public void setStart(AiManager newManager, AiCarInformation info,CarBodyScript newBodyScript)//start function gets called from the controller of the car
    {
        managerScript = newManager;
        carInfo = info;
        bodyScript = newBodyScript;
    }

    public override State runThisState()
    {
        if(canSeePlayer)
        {
            chase();
            return this;
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
        patrolState.setDest(managerScript.getClosedNext(transform.root.transform));
    }

    public void setStartState(Transform newTarget)
    {
        canSeePlayer = true;
        agent.speed = Random.Range(carInfo.chaseSpeed.x,carInfo.chaseSpeed.y);//sets chase speed
        target = newTarget;
      
        bodyScript.setChase(true);//turns on chase lights
    }

    public void outOfView()
    {
        canSeePlayer = false;
        bodyScript.setChase(false);//turns off chase lights
    }
    
}
