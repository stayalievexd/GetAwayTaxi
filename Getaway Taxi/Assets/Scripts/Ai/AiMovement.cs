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
    [Header("AI Settings")]

    [Tooltip("Main AI Manager Script")]
    [SerializeField] private float stoppingDistance = 5;

    [Header("AI Components")]

    [Tooltip("Nav Mesh Agent of the Ai")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Private Data")]
    private AiManager aiManagerScript;
    private AiCarInformation currentInfo;
    private int state = 0;
    private Transform currentPos = null;
    private Vector2 goV2;
    private float remainDistance;//so i can see it in the debugger

    public void setStart(AiManager managerScript,AiCarInformation info)//start function gets called from the controller of the car
    {
        aiManagerScript = managerScript;
        currentInfo = info;
        setStats();
        setPoint();
    }

    private void setStats()
    {
        //sets navmesh agent data with the currentInfo
    }

    void Update()
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
        // remainDistance = Vector2.Distance(currentV2,goV2);
        remainDistance = (goV2-currentV2).magnitude;
        
        return remainDistance;
    }

    public void setPoint()
    {
        Transform newPostition = aiManagerScript.getNewPoint(currentPos);
        agent.SetDestination(newPostition.position);
        currentPos = newPostition;
        goV2 = new Vector2(currentPos.position.x,currentPos.position.z);
    }
}
