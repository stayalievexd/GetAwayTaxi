using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] GameController controllerScript;

    [Header("Private data")]
    private bool ended = false;
 
    private void OnTriggerEnter(Collider other)
    {
        if(!ended)
        {
            if(other.transform.root.tag == "Player")
            {
                end(true);
            }
        }
    }

    public void end(bool active)
    {
        ended = active;
        controllerScript.reachedEnd();
    }
}
