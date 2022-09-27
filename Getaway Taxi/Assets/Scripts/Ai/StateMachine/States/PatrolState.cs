using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public bool canSeePlayer;
    public ChaseState ChaseState;
    public override State runThisState()
    {
        if(canSeePlayer)
        {
            return ChaseState;
        }
        else
        {
            return this;
        }
    }
}
