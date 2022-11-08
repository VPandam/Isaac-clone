using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float _speed;
    GameObject player;
    GameObject enemy;
    Rigidbody2D _rb;
    public Vector2 _shotDirection;
    void Start()
    {
        Destroy(gameObject, 5f);
        _rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        // _rb.velocity = shotDirection * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _shotDirection * Time.fixedDeltaTime * _speed);
    }

    public void SetBullet(Vector2 shotDirection, float speed)
    {
        _shotDirection = shotDirection;
        _speed = speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriger");
        if (other.tag.Equals("Enemy") || other.tag.Equals("Wall"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage();
                enemy.StartCoroutine(enemy.Knockback(
                    (enemy.transform.position - transform.position).normalized * 1));
            }
            Destroy(gameObject);
        }
    }
}