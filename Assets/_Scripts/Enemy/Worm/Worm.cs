using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;


public class Worm : Enemy
    {
        bool shooting, moving, movingHorizontal = true, callingMove;
        [SerializeField] float minFireRate, maxFireRate, shootSpeed;

        [SerializeField] GameObject rightShootingStart, leftShootingStart, bulletPrefab;

        private AIDestinationSetter _aiDestinationSetter;



        protected override void FixedUpdate()
        {
            if (!changingDirection)
                StartCoroutine(ChangeDirection());
            if (!callingMove)
                StartCoroutine(CallMove());
            if (moving)
                Move();
            if (!shooting)
                StartCoroutine(CallShoot());
            
        }
        // /// <summary>
        // /// Returns true if there is a collision
        // /// </summary>
        // /// <returns></returns>
        // bool CheckCircle()
        // {
        //     RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, raycastRange,
        //         Vector2.zero, 0f, roomMask );
        //
        //     if (raycastHit && !changingDirectionCollision)
        //     {
        //         Debug.Log(raycastHit.ToString() + " Worm collided with");
        //         Debug.Log(raycastHit.point);
        //         Vector2 directionToCollider = raycastHit.point - (Vector2)transform.position;
        //         directionToCollider = resources.GetCardinalDirection(directionToCollider);
        //         Debug.Log(directionToCollider.normalized);
        //         StartCoroutine(ChangeDirectionCollision(directionToCollider.normalized));
        //     }
        //
        //     return raycastHit != null;
        // }

        IEnumerator CallMove()
        {
            callingMove = true;
            moving = true;
            float timeForNextMove = 0.7f;
            float timeStopped = 0.1f;
            yield return new WaitForSeconds(timeForNextMove);
            moving = false;
            yield return new WaitForSeconds(timeStopped);
            callingMove = false;
        }
        void Move()
        {
            _rb.MovePosition(_rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
        IEnumerator ChangeDirection()
        {
            changingDirection = true;
            moveDirection = resources.GetRandomCardinalDirection();
            FlipSprite();
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            changingDirection = false;
        }
        IEnumerator ChangeDirectionCollision(Vector2 collisionDirection)
        {
            moveDirection = -collisionDirection;
            FlipSprite();
            yield return new WaitForSeconds(0.2f);
        }
        IEnumerator CallShoot()
        {
            shooting = true;
            Shoot();
            float timeForNextShoot = Random.Range(minFireRate, maxFireRate);
            yield return new WaitForSeconds(timeForNextShoot);
            shooting = false;
        }
        void Shoot()
        {
            if (movingHorizontal)
            {
                //Shoot a bullet to the right
                GameObject bulletRight = Instantiate(bulletPrefab, rightShootingStart.transform.position, Quaternion.identity);
                bulletRight.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.right, shootSpeed, attackDamage, rangeAttack);

                //Shoot a bullet to the left
                GameObject bulletLeft = Instantiate(bulletPrefab, leftShootingStart.transform.position, Quaternion.identity);
                bulletLeft.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.left, shootSpeed, attackDamage, rangeAttack);
            }
            else
            {
                //Shoot a bullet up
                GameObject bulletUp = Instantiate(bulletPrefab, rightShootingStart.transform.position, Quaternion.identity);
                bulletUp.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.up, shootSpeed, attackDamage, rangeAttack);

                //Shoot a bullet down
                GameObject bulletDown = Instantiate(bulletPrefab, leftShootingStart.transform.position, Quaternion.identity);
                bulletDown.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.down, shootSpeed, attackDamage, rangeAttack);
            }
        }

        void FlipSprite()
        {
            if (Mathf.Abs(moveDirection.y) == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                movingHorizontal = false;
            }
            else
            {
                movingHorizontal = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                _spriteRenderer.flipX = moveDirection.x > 0;
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Room"))
            {
                Vector2 directionToCollider = collision.GetContact(0).point - (Vector2)transform.position;
                directionToCollider = resources.GetVectorDirection(directionToCollider);
                Debug.Log(directionToCollider.normalized);
                StartCoroutine(ChangeDirectionCollision(directionToCollider.normalized));
            }

        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _rb.velocity = Vector2.zero;
                moving = false;
                StartCoroutine(CallMove());
            }
            
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawSphere(transform.position, raycastRange);
        // }
    }

