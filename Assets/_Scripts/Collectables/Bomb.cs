using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float exploteTime;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosionGO;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField]int bombDamage;
    private float actualScale, targetScale; 
    
    
    private void Start()
    {
        Invoke("Explode", exploteTime);
        actualScale = 1;
        targetScale = actualScale * 1.2f;
        StartCoroutine(nameof(GrowScale));
    }

    IEnumerator GrowScale()
    {
        while (true)
        {
            while (actualScale < targetScale)
            {
                actualScale += actualScale * Time.deltaTime * 0.4f;
                transform.localScale = new Vector3(1,1,1) * actualScale;
                yield return null;
            }

            while (actualScale > 1)
            {
                actualScale -= actualScale * Time.deltaTime * 0.4f;
                transform.localScale = new Vector3(1,1,1) * actualScale;
                yield return null;
            }
        }
    }


    void Explode()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var collider in collider2Ds)
        {
            IExplodable explodable = collider.gameObject.GetComponent<IExplodable>();
            if (explodable != null)
            {
                explodable.Explode(bombDamage);
                Debug.Log(collider.gameObject.name);
            }
        }

        Instantiate(explosionGO, transform.position, Quaternion.identity);
        PlayerManager.sharedInstance.playerAudioSource.PlayOneShot(explosionSound, 0.5f);
        Destroy(gameObject);  
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
