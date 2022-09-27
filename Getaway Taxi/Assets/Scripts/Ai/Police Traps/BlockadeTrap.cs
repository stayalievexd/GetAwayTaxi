using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockadeTrap : MonoBehaviour
{
    [Header("Blockade Information")]

    [Tooltip("")]
    private Transform[] spawnPoints;
    private Transform[] goPoints;
    private AiCarInformation[] spawnCars;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.gameObject.tag == "Player")
        {
            Debug.Log("Entered");
        }
    }
}
