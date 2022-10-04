using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPowers : MonoBehaviour
{
    [Header("Special Settings")]

    [Tooltip("Max Energy for the specials")]
    [SerializeField] private float maxEnergy = 100;

    [Tooltip("Starting Energy for the specials")]
    [SerializeField] private float startEnergy = 50;

    [Tooltip("Gain amount of the energy per second")]
    [SerializeField] private float gainAmount = 5f;

    [Tooltip("Time specials are delayed")]
    [SerializeField] private float delayTime = 2f;

    [Header("Emp Settings")]
    [SerializeField] private float empEnergyCost = 100.0f;
    [SerializeField] private float empRange = 400.0f;
    [SerializeField] private float empTime = 5.0f;
    [SerializeField] private Vector2 empTimeEffect = new Vector2(0.1f,1.5f);

    [Header("Private data")]
    private float energy = 100;
    private bool delay = false;
    private bool emp = false;
    private bool started = false;
    
    [Header("Private Scripts")]
    private GameController gameScript;
    private TimeManager timeScript;
    private CarUI uiScript;

    public void setStart(GameController newController,TimeManager newTimeScript,CarUI newUIScript)
    {
        gameScript = newController;
        timeScript = newTimeScript;
        uiScript = newUIScript;

        energy = startEnergy;

        uiScript.setMaxSlider(maxEnergy,energy);
    }

    void Update()
    {
        if(!Values.pauzed)
        {
            if(started)
            {
                if(!delay)
                {
                    if(Input.GetKeyDown(KeyCode.P))
                    {
                        activateEmp();
                    }
                    gainEnergy(gainAmount * Time.deltaTime);
                }
            }
        }
    }

    public void setStarted(bool active)
    {
        started = active;
    }

    private void gainEnergy(float addAmount)
    {
        if(energy + addAmount < maxEnergy)
        {
            energy += addAmount;
        }
        else
        {
            energy = maxEnergy;
        }
        uiScript.setEnergy(energy);
    }

    private void activateEmp()
    {
        if(energy >= empEnergyCost)
        {
            energy -= empEnergyCost;
            delay = true;
            Invoke("deactiveDelay",delayTime);
            activateTimeEffect();
        }
    }

    private void deactiveDelay()
    {
        delay = false;
    }

    private void activateTimeEffect()
    {
        StartCoroutine(timeScript.slowlySlowmo(empTimeEffect.y,empTimeEffect.x,5));
    }

    private void returnToNormal()
    {

    }
}
