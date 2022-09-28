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
    [SerializeField] private bool sceneAi = false;

    [Header("AI Components")]

    [Tooltip("Nav Mesh Agent of the Ai")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AiManager aiManagerScript;
    [SerializeField] private AiCarInformation currentInfo;

    [Header("Private Data")]
    private Transform currentPos = null;
    private Vector2 goV2;
    private float remainDistance;//so i can see it in the debugger

    public void Start()
    {
        if(sceneAi)
        {
            setPoint();
        }
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
