using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{

    [Header("AI Settings")]

    [Tooltip("Main AI Manager Script")]
    [SerializeField] private float stoppingDistance = 5;

    [Header("AI Components")]

    [Tooltip("Nav Mesh Agent of the Ai")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Private Data")]
    private AiManager aiManagerScript;
    private AiCarInformation carInfo;
    private Transform currentPos = null;
    private Vector2 goV2;
    private float remainDistance;//so i can see it in the debugger
    private float patrolSpeed = 0;

    [Header("Statemachine public data")]
    public bool canSeePlayer;
    public ChaseState ChaseState;

    public void setStart(AiManager managerScript,AiCarInformation info)//start function gets called from the controller of the car
    {
        carInfo = info;
        aiManagerScript = managerScript;
        setStats();
        setPoint();
    }

    private void setStats()
    {
        setRandomSpeed();
    }

    private void setRandomSpeed()
    {
        patrolSpeed = Random.Range(carInfo.patrolSpeed.x,carInfo.patrolSpeed.y);
        agent.speed = patrolSpeed;
    }

    public override State runThisState()//update function of the state machine
    {
        if(canSeePlayer)
        {
            return ChaseState;
        }
        else
        {
            movement();

            return this;
        }
    }

    private void movement()
    {
        if(currentPos != null)
        {
            if(getDistance() <= stoppingDistance)
            {
                setPoint();
            }
        }
    }

    private float getDistance()
    {
        Vector2 currentV2 = new Vector2(transform.position.x,transform.position.z);
        remainDistance = (goV2-currentV2).magnitude;
        
        return remainDistance;
    }

    public void setPoint()
    {
        Transform newPostition = aiManagerScript.getNewPoint(currentPos);
        agent.SetDestination(newPostition.position);
        setRandomSpeed();
        currentPos = newPostition;
        goV2 = new Vector2(currentPos.position.x,currentPos.position.z);
    }
}
