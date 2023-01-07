using System.Collections;
using Pathfinding;
using UnityEngine;

public class Chaser : Enemy
{
    AIDestinationSetter _destinationSetter;

    void FixedUpdate()
    {
        if (isKnockback)
        {
            return;
        }

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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        currentHp = maxHp;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = _spriteRenderer.sprite;
        _rb = GetComponent<Rigidbody2D>();
        moveDirection = -transform.right;
        _destinationSetter = GetComponent<AIDestinationSetter>();
    }
}
        






