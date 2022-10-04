using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    // [Header("Components")]

    [Header("Objects")]

    [Tooltip("Reverse camera")]
    [SerializeField] private GameObject[] reverseCamera;
    // [SerializeField] private GameObject reverseCamera;
    [SerializeField] private float turnOffTime = 1.5f;

    [Header("Ui objects")]

    [Tooltip("Indicator to show the energy of the car")]
    [SerializeField] Slider energySlider;

    [Tooltip("Text object of the speed counter")]
    [SerializeField] TMPro.TextMeshProUGUI speedCounter;

    [Tooltip("Text object of the acceleration")]
    [SerializeField] TMPro.TextMeshProUGUI accelearation;

    [Tooltip("Text object of the moved distance counter")]
    [SerializeField] TMPro.TextMeshProUGUI movedDistance;

    [Tooltip("Text object of the acceleration")]
    [SerializeField] TMPro.TextMeshProUGUI startUi;

    [Header("Private Scripts")]
    private CarStats statsScript;

    [Header("Private data")]
    private bool carActive = false;
    private int gearArrayID = 1;
    private string[] directions = {"R","N","F"};

    public void setStart(CarStats newStats)
    {
        Screen.lockCursor = true; 
        statsScript = newStats;
    }

    private void Update()
    {
        if(!Values.pauzed)
        {
            if(carActive)
            {
                setUI();
            }
        }
    }

    public void activateCar(bool active)
    {
        carActive = active;
        reverseCamera[1].SetActive(active);
        reverseCamera[2].SetActive(!active);
        startUi.gameObject.SetActive(!active);
    }


    public void setGear(int gear)
    {
        gearArrayID = gear+1;

        if(gear != -1)
        {
            if(reverseCamera[0].activeSelf && !IsInvoking("turnOfMirror"))
            {
                Invoke("turnOfMirror",turnOffTime);
            }
        }
        else
        {
            CancelInvoke("turnOfMirror");
            reverseCamera[0].SetActive(true);
            reverseCamera[1].SetActive(false);
        }
    }

    public void turnOfMirror()
    {
        reverseCamera[0].SetActive(false);
        reverseCamera[1].SetActive(true);
    }   

    private void setUI()
    {
        movedDistance.text = "M: " + statsScript.getMovedDistance().ToString("F0");
        speedCounter.text = directions[gearArrayID] + statsScript.getSpeed().ToString("F1");
        accelearation.text = statsScript.getAccel().ToString("F1");
    }

    public void setMaxSlider(float maxAmount,float currentAmount)//gets called from the special script sets the starting data for the slider
    {
        energySlider.maxValue = maxAmount;
        setEnergy(currentAmount);
    }

    public void setEnergy(float newValue)
    {
        energySlider.value = newValue;
    }
}
