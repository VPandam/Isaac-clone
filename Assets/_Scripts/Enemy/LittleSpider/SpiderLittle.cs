using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderLittle : Enemy
{
    //Animator params
    const string HIT = "Hit";
    
    [SerializeField]private float minMovementTime, maxMovementTime;

    private float distanceToPlayer;
    private bool attackPlayer, moving;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < rangeAttack) attackPlayer = true;
        else attackPlayer = false;

        if (!moving) StartCoroutine(nameof(Move));
    }

    IEnumerator Move()
    {
        moving = true;
        yield return new WaitForSeconds(.5f);
        float randomX, randomY;
        randomX = Random.Range(-1f, 1f);
        randomY = Random.Range(-1f, 1f);
        Vector2 randomDirection = new Vector2(randomX, randomY);

        float movementTime = Random.Range(minMovementTime, maxMovementTime);

        Vector2 playerDirection = player.transform.position - transform.position;
        while (movementTime > 0)
        {
            Debug.Log("while");
            if (attackPlayer)
            {
                _rb.MovePosition(_rb.position + playerDirection.normalized * Time.fixedDeltaTime * speed);
                movementTime -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate(); 
            }
            else
            {
                _rb.MovePosition(_rb.position + randomDirection.normalized * Time.fixedDeltaTime * speed);
                movementTime -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        yield return new WaitForSeconds(.5f);
        moving = false;
    }

    protected override IEnumerator BlinkColorDamage()
    {
        _animator.SetBool(HIT, true);
        yield return new WaitForSeconds(0.07f);
        _animator.SetBool(HIT, false);

    }
}
