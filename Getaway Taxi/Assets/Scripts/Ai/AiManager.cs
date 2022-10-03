using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiManager : MonoBehaviour
{   
    [Header("Nav Information")]

    [Tooltip("Route positions")]
    [SerializeField] private List<Transform> routePoints = new List<Transform>();

    [Header("Cops Spawn Settings")]
    
    [Tooltip("Time between new car spawns")]
    [SerializeField] private float copSpawnTime = 0.2f;

    [Tooltip("Max amount cops spawned cars in the world")]
    [SerializeField] private int maxCops = 50;

    [Tooltip("Cop Spawn positions")]
    [SerializeField] private List<Transform> copSpawns = new List<Transform>();

    [Tooltip("Spawn car informations")]
    [SerializeField] private AiCarInformation[] copAis;
    
    [Header("Civ Spawn Settings")]

    [Tooltip("Time between new car spawns")]
    [SerializeField] private float timeBetweenSpawns = 0.2f;

    [Tooltip("Max amount of default spawned cars on each spawn height")]
    [SerializeField] private int[] maxCiv = {100,50,100,100};

    [Tooltip("Spawn positions")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Tooltip("Spawn car informations")]
    [SerializeField] private AiCarInformation[] civAi;

    [Tooltip("Base movement car")]
    [SerializeField] private GameObject spawnCarObj;


    [Header("Private Information")]
    private List<Transform> spawnedCars = new List<Transform>();
    private List<Transform> spawnedCops = new List<Transform>();
    private List<int> aiRarities = new List<int>();
    private List<int> copRarities = new List<int>();
    private int currentHeight = 1;

    private void Start()
    {
        setAiRarities();
        startSpawn();
    }

    /////////////spawning "Ai" ///has duplicate code for now can be better optimized 

    private void setAiRarities()//makes a list with all ids of the ai's for a simple rarity effect
    {
        for(int i=0; i<civAi.Length; i++)
        {
            for(int b=0; b<civAi[i].rarity; b++)
            {
                aiRarities.Add(i);
            }
        }

        for(int i=0; i<copAis.Length; i++)
        {
            for(int b=0; b<copAis[i].rarity; b++)
            {
                copRarities.Add(i);
            }
        }
    }

    //start function
    private void startSpawn()
    {
        for(int i=0; i<spawnPoints.Count; i++)
        {
            spawnCar(i);
        }

        for(int i=0; i<copSpawns.Count; i++)
        {
            spawnCop(i);
        }

        Invoke("randomCopSpawn",copSpawnTime);
        Invoke("spawnRandomSpot",timeBetweenSpawns);
    }


    //invokes
    private void spawnRandomSpot()
    {
        int spawnPoint = Random.Range(0,spawnPoints.Count-1);
        Invoke("spawnRandomSpot",timeBetweenSpawns);
        spawnCar(spawnPoint);
    }

    private void randomCopSpawn()
    {
        int spawnPoint = Random.Range(0,copSpawns.Count-1);
        Invoke("randomCopSpawn",copSpawnTime);
        spawnCop(spawnPoint);
    }

    private void spawnCar(int spawnPoint)
    {
        int b = maxCiv.Length;
        for(int i=0; i<maxCiv.Length; i++)
        {
            if(maxCiv[i] > 0)
            {
                maxCiv[i] --;
                b--;

                Transform spawnPos = spawnPoints[spawnPoint].GetChild(i);
                
                AiCarInformation currentAi = civAi[aiRarities[Random.Range(0,aiRarities.Count)]];

                Transform spawnedAi = Instantiate(spawnCarObj,spawnPos.position,spawnPos.rotation).transform;
                Transform startDes = spawnPoints[spawnPoint].GetComponent<NextPoint>().nextPoint();

                AiController controllerScript = spawnedAi.GetComponent<AiController>();
                controllerScript.setStartInformation(currentAi,this,startDes);

                spawnedCars.Add(spawnedAi);
            }
        }

        if(b == maxCiv.Length)//if all spawned
        {
            CancelInvoke("spawnRandomSpot");
        }
    }

    private void spawnCop(int spawnPoint)
    {
        if(maxCops > 0)
        {
            maxCops --;

            Transform spawnPos = copSpawns[spawnPoint];
                
            AiCarInformation currentAi = copAis[copRarities[Random.Range(0,copRarities.Count)]];
            
            Transform spawnedAi = Instantiate(spawnCarObj,spawnPos.position,spawnPos.rotation).transform;
            Transform startDes = copSpawns[spawnPoint].GetComponent<NextPoint>().nextPoint();

            AiController controllerScript = spawnedAi.GetComponent<AiController>();
            controllerScript.setStartInformation(currentAi,this,startDes);

            spawnedCops.Add(spawnedAi);
        }
    }

    public Transform getClosedNext(Transform carTrans)
    {
        Transform closedPos = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = carTrans.position;
        foreach (Transform pos in routePoints)
        {
            float dist = Vector3.Distance(pos.position, currentPos);
            if (dist < minDist)
            {
                closedPos = pos;
                minDist = dist;
            }
        }
        
        return closedPos;
    }

    /* the old point system where they get a random point assigned */
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


    public void setHeight(int newHeight)
    {
        currentHeight = newHeight;
    }
}
