using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamState : State
{
    public PatrolState patrolState;
    public bool canSeePlayer;

    public override State runThisState()
    {
        if(!canSeePlayer)
        {
            return patrolState;
        }
        else
        {
           
            return this;
        }
    }
}
