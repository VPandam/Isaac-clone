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
    private float actualScaleMultiplier, targetScaleMultiplier;
    private Vector3 actualScale;
    
    
    private void Start()
    {
        Invoke("Explode", exploteTime);
         actualScale = transform.localScale;
        actualScaleMultiplier = 1;
        targetScaleMultiplier = actualScaleMultiplier * 1.2f;
        StartCoroutine(nameof(GrowScale));
    }

    IEnumerator GrowScale()
    {
        while (true)
        {
            while (actualScaleMultiplier < targetScaleMultiplier)
            {
                actualScaleMultiplier += actualScaleMultiplier * Time.deltaTime * 0.4f;
                transform.localScale =  actualScale * actualScaleMultiplier;
                yield return null;
            }

            while (actualScaleMultiplier > 1)
            {
                actualScaleMultiplier -= actualScaleMultiplier * Time.deltaTime * 0.4f;
                transform.localScale = actualScale * actualScaleMultiplier;
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
