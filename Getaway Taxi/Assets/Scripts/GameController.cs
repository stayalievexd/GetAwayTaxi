using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("End Settings")]

    [Tooltip("Time for the slow motion end effect")]
    [SerializeField] private float slowMoTime = 2f;

    [Tooltip("Min time speed of the end slowmotion")]
    [SerializeField] private float minSlowmo = 0.05f;

    [Header("Scripts")]
    [SerializeField] private AiManager aiMangerScript;
    [SerializeField] private CarController carScript;
    [SerializeField] private TimeManager timeScript;
    [SerializeField] private EndPoint endScript;
    [SerializeField] private FakeSteeringWheel steerinScript;

    [Header("Private Data")]
    private bool reached = false;

    public void cought()
    {

    }

    public void resetLevel()
    {

    }

    public void reachedEnd()
    {
        reached = true;
        StartCoroutine(timeScript.slowlySlowmo(slowMoTime,minSlowmo,1));
    }

    public void pauzedGame(bool active)
    {
        Debug.Log("Pauzed : " + active);
        Values.pauzed = active;
    }


    /////get data

    public TimeManager getTimeScript()
    {
        return timeScript;
    }

    public AiManager getAiManager()
    {
        return aiMangerScript;
    }

    public FakeSteeringWheel getSteering()
    {
        return steerinScript;
    }
}
