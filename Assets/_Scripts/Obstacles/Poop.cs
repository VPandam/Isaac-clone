using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
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
                Destroy(col.gameObject);
                Destroy(gameObject);
            }
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
}
