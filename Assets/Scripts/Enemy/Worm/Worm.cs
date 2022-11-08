using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Enemy
{
    bool shooting;
    [SerializeField] float minFireRate, maxFireRate, shootSpeed;

    [SerializeField] GameObject rightShootingStart, leftShootingStart, bulletPrefab;



    private void FixedUpdate()
    {
        Move();
        if (!shooting)
            StartCoroutine(CallShoot());
    }

    void Move()
    {
        _rb.MovePosition(_rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }
    IEnumerator ChangeDirection()
    {
        changingDirection = true;
        yield return null;
        changingDirection = false;
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
        //Shoot a bullet to the right
        GameObject bulletRight = Instantiate(bulletPrefab, rightShootingStart.transform.position, Quaternion.identity);
        bulletRight.GetComponent<EnemyBullet>().SetBullet(Vector2.right, shootSpeed, attackDamage);

        //Shoot a bullet to the left
        GameObject bulletLeft = Instantiate(bulletPrefab, leftShootingStart.transform.position, Quaternion.identity);
        bulletLeft.GetComponent<EnemyBullet>().SetBullet(Vector2.left, shootSpeed, attackDamage);


    }
}
