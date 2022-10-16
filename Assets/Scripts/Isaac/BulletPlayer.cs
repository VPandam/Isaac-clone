using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float speed = 3f;
    GameObject player;
    GameObject enemy;
    void Start()
    {
        Destroy(gameObject, 5f);
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(new Vector3(PlayerManager.instance.shotSpeed * Time.deltaTime, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriger");
        if (other.tag.Equals("Enemy") || other.tag.Equals("Wall"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy)
                enemy.TakeDamage();
            Destroy(gameObject);
        }
    }
}