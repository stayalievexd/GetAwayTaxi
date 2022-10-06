using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    /*
        Slow motion effect for the end when end goal is reached

        Or maybe for drifting?
    */

    [Header("Scripts")]

    [Tooltip("The main controller of the game")]
    [SerializeField] private GameController controllerScript;
    
    [Header("Settings")]

    [Tooltip("The speed of the sped up time")]
    [SerializeField] private float speedUpTime = 2f;
    
    [Tooltip("The speed of the slowmotion time")]
    [SerializeField] private float slowMoTime = 0.05f;

    [Tooltip("The speed of the normal time")]
    [SerializeField] private float normalTime = 1;

    [Header("Private data")]
    private bool slowMoActive = false;
    private bool pauzed = false;
    private bool speedUp = false;

    public void Start()
    {
        checkEndTime(0);//resets time to make sure its at normal speed
    }

    public void pauzeGame(bool active)
    {
        if(active)
        {
            checkEndTime(1);
        }
        else
        {
            checkEndTime(0);
        }
    } 

    public void fasterTime(bool active)
    {
        if(active)
        {
            checkEndTime(3);
        }
        else
        {
            checkEndTime(0);
        }
    }

    public void slowMotion(bool active)
    {
        if(active)
        {
            checkEndTime(2);
        }
        else
        {
            checkEndTime(0);
        }
    }
    
    private void checkEndTime(int resetTime)
    {
        slowMoActive = false;
        pauzed = false;
        speedUp = false;
        if(resetTime == 0)//normal time
        {
            Time.timeScale = normalTime;
            Time.fixedDeltaTime = 0.01388889f;
            controllerScript.pauzedGame(false);
        }
        else if(resetTime == 1)//pauze time
        {
            pauzed = true;
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0.01388889f;
            controllerScript.pauzedGame(true);
        }
        else if(resetTime == 2)//slow mo time
        {
            slowMoActive = true;
            Time.timeScale = slowMoTime;
            Time.fixedDeltaTime = Time.timeScale * 0.01388889f;
            controllerScript.pauzedGame(false);
        }
        else if(resetTime == 3)//speed up time
        {
            speedUp = true;
            Time.timeScale = speedUpTime;
            Time.fixedDeltaTime = 0.01388889f;
            controllerScript.pauzedGame(false);
        }
        //4 = keep same time
    }

    //SLowly changes the speed of time 
    public IEnumerator slowlySlowmo (float duration, float minTimeSpeed,int resetTime)
    {
        float startTime = Time.timeScale;
        float elepsed = 0.0f;
        float currentTime = startTime;

        while(elepsed < duration)
        {
            elepsed += Time.unscaledDeltaTime;

            currentTime = Mathf.Lerp(currentTime, minTimeSpeed, duration * Time.unscaledDeltaTime);
            Time.timeScale = currentTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            yield return null;
        }

        if(resetTime == 5)
        {
            StartCoroutine(slowlySlowmo(duration/3,startTime,0));
        }
        else
        {
            checkEndTime(resetTime);
        }
    }


    //for when going to another scene
    public void stopTimeChanges()
    {
        slowMoActive = false;
        pauzed = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
