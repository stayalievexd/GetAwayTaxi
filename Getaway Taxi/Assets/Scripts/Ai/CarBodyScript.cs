using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBodyScript : MonoBehaviour
{
    [SerializeField] private Animator chaseAnim;

    public void setChase(bool active)
    {
        if(chaseAnim)
        {
            chaseAnim.SetBool("Chase",chaseAnim);
        }
    }
}
