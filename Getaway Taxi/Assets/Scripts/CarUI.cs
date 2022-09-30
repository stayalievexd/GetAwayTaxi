using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    [Header("Components")]

    [Tooltip("Gear stick Animator")]
    [SerializeField] private Animator gearAnim;

    [Header("Objects")]

    [Tooltip("Reverse camera")]
    [SerializeField] private GameObject reverseCamera;
    [SerializeField] private GameObject screenPlaceholder;
    [SerializeField] private float turnOffTime = 1.5f;

    [Header("Scripts")]

    [Tooltip("Car stats script")]
    [SerializeField] private CarStats statsScript;

    [Header("Text objects")]

    [Tooltip("Text object of the speed counter")]
    [SerializeField] TMPro.TextMeshProUGUI speedCounter;

    [Tooltip("Text object of the acceleration")]
    [SerializeField] TMPro.TextMeshProUGUI accelearation;

    [Tooltip("Text object of the moved distance counter")]
    [SerializeField] TMPro.TextMeshProUGUI movedDistance;

    [Header("Private data")]

    private int gearArrayID = 1;
    private string[] directions = {"R","N","F"};

    private void Start()
    {
        Screen.lockCursor = true; 
    }

    private void Update()
    {
        setUI();
    }

    public void setGear(int gear)
    {
        gearArrayID = gear+1;
        gearAnim.SetInteger("Gear",1);
        // reverseCamera.SetActive(gear == -1);
        // screenPlaceholder.SetActive(gear != -1);

        if(gear != -1)
        {
            if(reverseCamera.activeSelf && !IsInvoking("turnOfMirror"))
            {
                Invoke("turnOfMirror",turnOffTime);
            }
        }
        else
        {
            CancelInvoke("turnOfMirror");
            reverseCamera.SetActive(true);
            screenPlaceholder.SetActive(false);
        }
    }

    public void turnOfMirror()
    {
        reverseCamera.SetActive(false);
        screenPlaceholder.SetActive(true);
    }   

    private void setUI()
    {
        movedDistance.text = "M: " + statsScript.getMovedDistance().ToString("F0");
        speedCounter.text = directions[gearArrayID] + statsScript.getSpeed().ToString("F1");
        accelearation.text = statsScript.getAccel().ToString("F1");
    }
}
