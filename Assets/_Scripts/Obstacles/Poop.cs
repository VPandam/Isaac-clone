using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Poop : Obstacle
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private List<GameObject> poopLootList;
    bool shaking;

    //Shaking
    float targetScale ;
    float originalScale;

    [SerializeField] private int hitsForDestroy = 3;
    private void Start()
    {
        targetScale = transform.localScale.x * 1.12f;
        originalScale = transform.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerBullet"))
        {
            _particleSystem.Play();
            StartCoroutine(nameof(ShakeOnHit));
            hitsForDestroy--;
            if(hitsForDestroy <= 0)
            {
                InstantiateLoot() ;
                Destroy(col.gameObject);
                Destroy(gameObject);
            }
        }
    }

    void InstantiateLoot()
    {
        //There is 1 chance between 20 to get loot of destroying a poo.
        if (Random.Range(0,20) == 19)
        {
            Instantiate(poopLootList[Random.Range(0, poopLootList.Count)], transform.position,
                Quaternion.identity, Resources.sharedInstance.lootGO.transform);
        }
    }

    IEnumerator ShakeOnHit()
    {
        if (!shaking)
        {
            shaking = true;
            
            float actualScale = 1;
            
            while (actualScale < targetScale)
            {
                actualScale += actualScale * Time.deltaTime * 0.8f;
                transform.localScale = new Vector3(1,1,1) * actualScale;
                yield return null;
            }

            while (transform.localScale.x > originalScale)
            {
                actualScale -= actualScale * Time.deltaTime * 0.8f;
                transform.localScale = new Vector3(1,1,1) * actualScale;
                yield return null;
            }

            shaking = false;
        }
    }

    public override void Explode(int damage)
    {
        InstantiateLoot();
        base.Explode(damage);
    }
}
