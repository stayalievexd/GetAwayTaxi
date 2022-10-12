using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{   
    [Header("SteeringWheelObjects")]
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private Transform[] handPos = new Transform[2];
    [SerializeField] private Transform[] holdPos = new Transform[2];
    [SerializeField] private Transform[] handVis = new Transform[2];

    [Header("Objects")]

    [SerializeField] private GameObject pcPlayer;
    [SerializeField] private GameObject vrPlayer;

    [Header("Scripts")]
    [SerializeField] private GameController controllerScript;
    [SerializeField] private Car movementScript;
    [SerializeField] private CarCollider colliderScript;
    [SerializeField] private CarUI uiScript;
    [SerializeField] private CarStats statScript;
    [SerializeField] private SpecialPowers specialScript;

    [Header("Private Data")]
    private bool started = false;

    void Start()
    {
        setStartData();
        // setStartVr();
    }

    private void setStartVr()
    {
        pcPlayer.SetActive(Application.isEditor);
        vrPlayer.SetActive(!Application.isEditor);
    }
   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) || OVRInput.GetDown(OVRInput.Button.One))
        {
            if(!Values.pauzed)
            {   
                startCar(!started);
            }
        }
    }

    private void setStartData()
    {
        controllerScript.getSteering().setStart(steeringWheel,handPos,handVis,holdPos);
        specialScript.setStart(controllerScript,controllerScript.getTimeScript(),uiScript);
        movementScript.setStart(uiScript,controllerScript.getAiManager(),controllerScript.getSteering());
        statScript.setStart(movementScript);
        colliderScript.setStartData(movementScript,statScript);
        uiScript.setStart(statScript);
    }

    private void startCar(bool active)
    {
        started = active;
        movementScript.startCar(active);
        uiScript.activateCar(active);
        specialScript.setStarted(active);
        //needs to trigger animation that makes the car shake 
    }
}
