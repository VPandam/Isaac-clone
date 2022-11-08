using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy
{




    void FixedUpdate()
    {
        Debug.Log(isKnockback);
        if (isKnockback)
        {
            return;
        }
        switch (_currentState)
        {
            case EnemyState.wander:
                Wander();
                if (!changingDirection)
                    CheckCircle();
                break;
            // case enemyState.follow:
            //     follow();
            //     break;
            case EnemyState.dead:
                Destroy(gameObject);
                break;

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
    public virtual void Wander()
    {
        if (!choosingDirection)
        {
            StartCoroutine(ChooseDirection());
        }
        _rb.MovePosition(_rb.position + moveDirection * speed * Time.fixedDeltaTime);




        // if (IsPlayerInAttackRange() && currentState != enemyState.dead)
        // {
        //     currentState = enemyState.follow;
        // }
    }
    public virtual IEnumerator ChooseDirection()
    {
        choosingDirection = true;

        moveDirection = GetRandomDirection();
        FlipSprite();
        yield return new WaitForSeconds(Random.Range(5f, 10f));

        choosingDirection = false;

    }

    // void CheckDirection()
    // {
    //     RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDirection, raycastRange, roomMask);
    //     if (raycastHit)
    //         StartCoroutine(ChangeDirection(raycastHit));
    // }

    /// <summary>
    /// Returns true if there is a collision
    /// </summary>
    /// <returns></returns>
    bool CheckCircle()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, raycastRange, roomMask);
        if (collider)
            StartCoroutine(ChangeDirection(collider));
        return collider != null;
    }

    /// <summary>
    /// Changes the direction of the enemy to the opposite of the actual
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public virtual IEnumerator ChangeDirection(Collider2D collider)
    {
        if (!changingDirection)
        {
            changingDirection = true;
            moveDirection = -moveDirection;
            FlipSprite();
            yield return new WaitForSeconds(1f);
            changingDirection = false;
        }
    }

    /// <summary>
    /// Changes the direction of the enemy to a random one 
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="moveDirection"></param>
    /// <returns></returns>
    public virtual IEnumerator ChangeDirection()
    {
        if (!changingDirection)
        {
            changingDirection = true;
            moveDirection = GetRandomDirection();
            FlipSprite();
            yield return new WaitForSeconds(1f);
            changingDirection = false;
        }
    }

    /// <summary>
    /// Returns true if the distance to the player is <=than our range attack
    /// </summary>
    bool IsPlayerInAttackRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= rangeAttack)
            return true;
        else
            return false;

    }


    /// <summary>
    /// Gets a random direction
    /// </summary>
    /// <returns></returns>
    Vector2 GetRandomDirection()
    {
        float movementX = Random.Range(-100, 101);
        float movementY = Random.Range(-100, 101);
        return new Vector2(movementX, movementY).normalized;
    }
    /// <summary>
    /// Checks if we are moving left or right and flips the sprite to tally.
    /// </summary>
    void FlipSprite()
    {
        if (moveDirection.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    // void follow()
    // {
    //     this.speed = 2f;
    //     transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
    // }
}




