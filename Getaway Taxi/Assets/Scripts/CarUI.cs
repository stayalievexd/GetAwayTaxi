using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    [Header("Scripts")]

    [Tooltip("Car stats script")]
    [SerializeField] private CarStats statsScript;

    [Header("Text objects")]

    [Tooltip("Text object of the speed counter")]
    [SerializeField] TMPro.TextMeshProUGUI speedCounter;

    [Tooltip("Text object of the moved distance counter")]
    [SerializeField] TMPro.TextMeshProUGUI movedDistance;

    [Header("Private data")]

    private int gearArrayID = 1;
    private string[] directions = {"R","N","F"};

    private void Start()
    {
        Screen.lockCursor = true; 
        // Cursor.visible = false;
    }

    private void Update()
    {
        setUI();
    }

    public void setGear(int gear)
    {
        gearArrayID = gear+1;
    }

    private void setUI()
    {
        movedDistance.text = "M: " + statsScript.getMovedDistance().ToString("F0");
        speedCounter.text = directions[gearArrayID] + statsScript.getSpeed().ToString("F1");
    }
}
