using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State
{

    [Header("AI Settings")]

    [Tooltip("Main AI Manager Script")]
    [SerializeField] private float stoppingDistance = 10;

    [Header("AI Components")]

    [Tooltip("Nav Mesh Agent of the Ai")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Private Data")]
    private AiCarInformation carInfo;
    private Transform currentPos = null;
    private Vector2 goV2;
    private float remainDistance;//so i can see it in the debugger
    private float patrolSpeed = 0;

    [Header("Statemachine public data")]
    public bool canSeePlayer;
    public ChaseState ChaseState;

    public void setStart(AiCarInformation info,Transform startDest)//start function gets called from the controller of the car
    {
        carInfo = info;
        setStats();
        setDest(startDest);
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
                setNextPoint();
            }
        }
    }

    private float getDistance()
    {
        Vector2 currentV2 = new Vector2(transform.position.x,transform.position.z);
        remainDistance = (goV2-currentV2).magnitude;
        
        return remainDistance;
    }   

    public void setNextPoint()
    {
        Transform newPostition = currentPos.GetComponent<NextPoint>().nextPoint();
        setDest(newPostition);
    }

    public void setDest(Transform newPostition)
    {
        currentPos = newPostition;
        goV2 = new Vector2(currentPos.position.x,currentPos.position.z);
        Vector3 newDest = new Vector3(currentPos.position.x,transform.root.transform.position.y,currentPos.position.z);
        agent.SetDestination(newDest);
        setRandomSpeed();
    }
}
