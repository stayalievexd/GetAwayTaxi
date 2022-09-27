using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public RamState ramState;
    public PatrolState patrolState;
    public bool isInAttackRage;
    public bool canSeePlayer;
    public override State runThisState()
    {
        if(canSeePlayer)
        {
            if(isInAttackRage)
            {
                return ramState;
            }
            else
            {
                return this;
            }
        }
        else
        {
            return patrolState;
        }
    }
}
