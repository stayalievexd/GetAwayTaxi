using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{   
    [Header("Nav Information")]

    [Tooltip("Route positions")]
    [SerializeField] private List<Transform> routePoints = new List<Transform>();

    [Tooltip("Player Car for the police to chase")]
    [SerializeField] private Transform playerCar;

    [Header("Spawn Settings")]

    [Tooltip("Time between new car spawns")]
    [SerializeField] private float timeBetweenSpawns = 2.0f;

    [Tooltip("Max amount of default spawned cars in the world")]
    [SerializeField] private int maxCiv = 300;

    [Tooltip("Max amount cops spawned cars in the world")]
    [SerializeField] private int maxCops = 300;

     [Header("Spawn Information")]
    
    [Tooltip("Base movement car")]
    [SerializeField] private GameObject spawnCarObj;

    [Tooltip("Spawn car informations")]
    [SerializeField] private AiCarInformation[] civAi;

    [Tooltip("Spawn car informations")]
    [SerializeField] private AiCarInformation[] copAis;

    [Tooltip("Spawn positions")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Private Information")]
    private List<Transform> spawnedCars = new List<Transform>();
    private List<Transform> spawnedCops = new List<Transform>();

    private List<int> aiRarities = new List<int>();
    private List<int> copRarities = new List<int>();

    private void Start()
    {
        setAiRarities();
        startSpawn();
    }

    private void setAiRarities()//makes a list with all ids of the ai's for a simple rarity effect
    {
        for(int i=0; i<civAi.Length; i++)
        {
            for(int b=0; b<civAi[i].rarity; b++)
            {
                aiRarities.Add(i);
            }
        }
    }

    public Transform getNewPoint(Transform lastPos)
    {
        Transform newReturn = null;
        if(lastPos == null)
        {
            newReturn = routePoints[Random.Range(0,routePoints.Count-1)];
        }
        else
        {
            while(newReturn == null || newReturn == lastPos)
            {
                newReturn = routePoints[Random.Range(0,routePoints.Count-1)];
            }
        }

        return newReturn;
    }

    private void startSpawn()
    {
        for(int i=0; i<spawnPoints.Count; i++)
        {
            spawnCar(i);
        }

        Invoke("spawnRandomSpot",timeBetweenSpawns);
    }

    private void spawnRandomSpot()
    {
        int spawnPoint = Random.Range(0,spawnPoints.Count-1);
        spawnCar(spawnPoint);
        Invoke("spawnRandomSpot",timeBetweenSpawns);
    }

    private void spawnCar(int spawnPoint)
    {
        for(int i=0; i<3; i++)
        {
            if(spawnedCars.Count < maxCiv)
            {
                Transform spawnPos = spawnPoints[spawnPoint].GetChild(i);
                
                AiCarInformation currentAi = civAi[aiRarities[Random.Range(0,aiRarities.Count)]];
               
                Transform spawnedAi = Instantiate(spawnCarObj,spawnPos.position,spawnPos.rotation).transform;

                AiController controllerScript = spawnedAi.GetComponent<AiController>();
                controllerScript.setStartInformation(currentAi,this);

                spawnedCars.Add(spawnedAi);
            }
            else
            {
                return;
            }
        }
    }

    public Transform getPlayer()
    {
        return playerCar;
    }

}
