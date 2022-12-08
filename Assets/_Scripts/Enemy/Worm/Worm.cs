using System.Collections;
using UnityEngine;


    public class Worm : Enemy
    {
        bool shooting, moving, movingHorizontal = true, callingMove, changingDirectionCollision;
        [SerializeField] float minFireRate, maxFireRate, shootSpeed;

        [SerializeField] GameObject rightShootingStart, leftShootingStart, bulletPrefab;


        private void Update()
        {
            if (isKnockback)
            {
                return;
            }
        }
        private void FixedUpdate()
        {
            if (isKnockback)
            {
                return;
            }

            if (!changingDirection)
                StartCoroutine(ChangeDirection());
            if (!callingMove)
                StartCoroutine(CallMove());
            if (moving)
                Move();
            if (!shooting)
                StartCoroutine(CallShoot());

            CheckCircle();

        }
        /// <summary>
        /// Returns true if there is a collision
        /// </summary>
        /// <returns></returns>
        bool CheckCircle()
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, raycastRange, roomMask);

            if (collider && !changingDirectionCollision)
            {
                Vector2 directionToCollider = collider.transform.position - transform.position;
                directionToCollider = resources.GetCardinalDirection(directionToCollider);
                Debug.Log(directionToCollider);
                StartCoroutine(ChangeDirectionCollision(directionToCollider.normalized));
            }

            return collider != null;
        }

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
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            changingDirection = false;
        }
        IEnumerator ChangeDirectionCollision(Vector2 direction)
        {
            changingDirectionCollision = true;
            moveDirection = direction;
            FlipSprite();
            yield return new WaitForSeconds(0.5f);
            changingDirectionCollision = false;
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
                bulletRight.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.right, shootSpeed, attackDamage);

                //Shoot a bullet to the left
                GameObject bulletLeft = Instantiate(bulletPrefab, leftShootingStart.transform.position, Quaternion.identity);
                bulletLeft.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.left, shootSpeed, attackDamage);
            }
            else
            {
                //Shoot a bullet up
                GameObject bulletUp = Instantiate(bulletPrefab, rightShootingStart.transform.position, Quaternion.identity);
                bulletUp.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.up, shootSpeed, attackDamage);

                //Shoot a bullet down
                GameObject bulletDown = Instantiate(bulletPrefab, leftShootingStart.transform.position, Quaternion.identity);
                bulletDown.GetComponent<EnemyTear>().SetEnemyBullet(Vector2.down, shootSpeed, attackDamage);
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

        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawSphere(transform.position, raycastRange);
        // }
    }

