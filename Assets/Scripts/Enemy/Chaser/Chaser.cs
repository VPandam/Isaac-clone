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
        /// Changes the direction of the enemy to a random one.
        /// If we pass a collider as a parameter changes the direction to the opposite of the actual.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public virtual IEnumerator ChangeDirection(Collider2D collider = null)
        {
            if (!changingDirection)
            {
                changingDirection = true;
                if (collider)
                    moveDirection = -moveDirection;
                else
                    moveDirection = GetRandomDirection();

                FlipSprite();

                yield return new WaitForSeconds(1f);
                changingDirection = false;
            }
        }

        /// <summary>
        /// Returns true if the distance to the player is <= than our range attack
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





