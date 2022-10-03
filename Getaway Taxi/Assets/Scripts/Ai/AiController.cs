using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    /*
        Controller script of individual ai
    */
    
    [Header("States data")]
    [SerializeField] private StateManager stateScript;
    [SerializeField] private PatrolState patrolState;
    [SerializeField] private ChaseState chaseState;

    [Header("Set private data")]
    [SerializeField] private AiLook lookScript;
    [SerializeField] private Transform bodyHolder;

    [Header("private data")]
    private GameObject spawnedBody;
    private AiCarInformation aiInformation;
    private AiManager managerScript;
    private CarBodyScript bodyScript = null;

    public void setStartInformation(AiCarInformation newInformation,AiManager newManager,Transform startPos)
    {   
        aiInformation = newInformation;
        managerScript = newManager;
        patrolState.setStart(newInformation,startPos);
        stateScript.setStart(this);
        spawncarBody();
        lookScript.enabled = newInformation.police;//disable for testing
    }

    public void spawncarBody()//spawns the body of the car
    {
        spawnedBody = Instantiate(aiInformation.spawnObject,bodyHolder.position,bodyHolder.rotation,bodyHolder);
        chaseState.setStart(managerScript,aiInformation,spawnedBody.GetComponent<CarBodyScript>());
    }

    public void crashed()
    {

    }
}
