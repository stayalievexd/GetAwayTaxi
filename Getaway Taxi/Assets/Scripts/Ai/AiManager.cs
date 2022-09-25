using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{   
    [Header("Nav Information")]

    [Tooltip("Route positions")]
    [SerializeField] private List<Transform> routePoints = new List<Transform>();

    [Header("Spawn Information")]

    [Tooltip("Time between new car spawns")]
    [SerializeField] private float timeBetweenSpawns = 2.0f;

    [Tooltip("Max amount of spawned cars in the world")]
    [SerializeField] private int maxSpawns = 300;

    [Tooltip("Spawn car informations")]
    [SerializeField] private AiCarInformation[] aiInformations;

    [Tooltip("Spawn positions")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Private Information")]
    private List<Transform> spawnedCars = new List<Transform>();
    private List<int> aiRarities = new List<int>();

    private void Start()
    {
        setAiRarities();
        startSpawn();
    }

    private void setAiRarities()//makes a list with all ids of the ai's for a simple rarity effect
    {
        for(int i=0; i<aiInformations.Length; i++)
        {
            for(int b=0; b<aiInformations[i].rarity; b++)
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

        Invoke("spawnCar",timeBetweenSpawns);
    }

    private void spawnRandomSpot()
    {
        int spawnPoint = Random.Range(0,spawnPoints.Count-1);
        spawnCar(spawnPoint);
    }

    private void spawnCar(int spawnPoint)
    {
        for(int i=0; i<3; i++)
        {
            if(spawnedCars.Count < maxSpawns)
            {
                Transform spawnPos = spawnPoints[spawnPoint].GetChild(i);
                
                AiCarInformation currentAi = aiInformations[aiRarities[Random.Range(0,aiRarities.Count)]];
               
                Transform spawnedAi = Instantiate(currentAi.spawnObject,spawnPos.position,spawnPos.rotation).transform;

                AiController controllerScript = spawnedAi.GetComponent<AiController>();
                controllerScript.setStartInformation(currentAi,this);

                spawnedCars.Add(spawnedAi);
            }
            else
            {
                return;
            }
        }

        Invoke("spawnCar",timeBetweenSpawns);
    }

}
