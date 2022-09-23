using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{   
    /*
        Needed:

        Patrouling the streets between points
    */

    [Header("AI Components")]

    [Tooltip("Main AI Manager Script")]
    [SerializeField] private AiManager aiManagerScript;
    
    [Tooltip("Nav Mesh Agent of the Ai")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Private Data")]
    private int state = 0;
    private Transform currentPos = null;

    void Start()
    {   
        setPoint();
    }

    void Update()
    {
        if(currentPos != null)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                setPoint();
            }
        }
    }

    public void setPoint()
    {
        agent.SetDestination(aiManagerScript.getNewPoint(currentPos).position);
    }
}
