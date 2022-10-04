using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private Animator indicatorAnim;
    [SerializeField] private Indicator behind;

    private int countTrigger = 0;
    private bool frontSet = false;
 
    void OnTriggerEnter(Collider other) 
    {
        if(countTrigger == 0)
        {
            if(behind)
            {
                behind.setBehind(true);
            }
            indicatorAnim.SetBool("Show",true);
        }
        countTrigger ++;
    }
 
    void OnTriggerExit(Collider other) 
    {
        if(countTrigger - 1 > 0)
        {
            countTrigger --;
        }
        else{
            countTrigger = 0;
            if(!frontSet)
            {
                if(behind)
                {
                    behind.setBehind(false);
                }
                indicatorAnim.SetBool("Show",false);
            }
        }
    }

    public void setBehind(bool active)
    {
        frontSet = active;
        if(active)
        {
            indicatorAnim.SetBool("Show",true);
        }
        else
        {
            if(countTrigger == 0)//if there is nothing in this trigger
            {
                indicatorAnim.SetBool("Show",false);
            }
        }
        if(behind)
        {
            behind.setBehind(active);
        }
    }

}
