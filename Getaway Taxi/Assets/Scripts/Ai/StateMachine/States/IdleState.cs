using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PatrolState patrolState;
    public bool idle;

    public override State runThisState()
    {
        if(!idle)
        {
            return patrolState;
        }
        else
        {
            return this;
        }
    }
}
