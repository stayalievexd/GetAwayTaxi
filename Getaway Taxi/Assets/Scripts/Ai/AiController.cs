using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    /*
        Controller script of individual ai
    */
    [Header("States data")]
    [SerializeField] private PatrolState patrolState;
    [SerializeField] private ChaseState chaseState;

    [Header("Set private data")]
    [SerializeField] private AiMovement movementScript;
    [SerializeField] private Transform bodyHolder;

    [Header("private data")]
    private GameObject spawnedBody;
    private AiCarInformation aiInformation;

    public void setStartInformation(AiCarInformation newInformation,AiManager managerScript)
    {   
        aiInformation = newInformation;
        patrolState.setStart(managerScript,newInformation);
        spawncarBody();
    }

    public void spawncarBody()//spawns the body of the car
    {
        spawnedBody = Instantiate(aiInformation.spawnObject,bodyHolder.position,bodyHolder.rotation,bodyHolder);
    }
}
