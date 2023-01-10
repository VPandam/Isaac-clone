using System.Collections;
using Pathfinding;
using UnityEngine;

public class Chaser : Enemy
{
    AIDestinationSetter _destinationSetter;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        switch (_currentState)
        {
            case EnemyState.wander:
                _destinationSetter.target = player.transform;

                // Wander();
                // if (!changingDirection)
                //     CheckCircle();
                break;
            // case enemyState.follow:
            //     follow();
            //     break;
        }

        // if (!IsPlayerInAttackRange() && currentState != enemyState.dead)
        // {
        //     currentState = enemyState.wander;
        // }
        // else if (IsPlayerInAttackRange() && currentState != enemyState.dead)
        // {
        //     currentState = enemyState.follow;
        // }
    }

    protected override void Start()
    {
        base.Start();
        _destinationSetter = GetComponent<AIDestinationSetter>();
    }
}
        






