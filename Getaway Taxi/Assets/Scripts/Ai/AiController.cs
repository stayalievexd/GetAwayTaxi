using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    /*
        Controller script of individual ai
    */

    [SerializeField] private AiMovement movementScript;

    public void setStartInformation(AiCarInformation newInformation,AiManager managerScript)
    {   
        movementScript.setStart(managerScript,newInformation);
    }
}
