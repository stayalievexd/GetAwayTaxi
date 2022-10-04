using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private State currentState;

    [Header("Switch from all States")]
    [SerializeField] private CrashedState crashedState;

    [Header("Switch all Values")]
    [SerializeField] private bool crashed = false;

    [Header("Private data")]
    private AiController controllerScript;

    public void setStart(AiController newScript)//start function gets called from the controller of the car
    {
        controllerScript = newScript;
    }

    private void Update()
    {
        if(!Values.pauzed)
        { 
            runStateMachine();
        }
    }

    private void runStateMachine()
    {
        checkAll();//checks states that can switch from any state

        State nextState = currentState?.runThisState();//if not null then run current state other wise dont run

        if(nextState != null)
        {
            SwitchStateNextState(nextState);
        }
    }

    private void checkAll()
    {
        if(crashed)
        {
            currentState = crashedState; 
        }
    }

    private void SwitchStateNextState(State nextState)
    {
        currentState = nextState;
    }


    public bool setCrashed(Vector3 addedForce)
    {
        if(!crashed)
        {
            crashedState.crash(addedForce);
            controllerScript.crashed();
            crashed = true;
        }
        
        return crashed;
    }
}
