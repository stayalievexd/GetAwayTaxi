using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBodyScript : MonoBehaviour
{
    [SerializeField] private GameObject iconObject;
    [SerializeField] private Animator chaseAnim;

    public void setChase(bool active)
    {
        if(chaseAnim)
        {
            chaseAnim.SetBool("Chase",chaseAnim);
        }
    }

    public void setIcon(bool active)
    {
        if(iconObject)
        {
            iconObject.SetActive(active);
        }
    }
}
