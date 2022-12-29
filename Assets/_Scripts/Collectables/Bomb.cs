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
        targetScale = actualScale * 2;
        StartCoroutine(nameof(GrowScale));
    }

    IEnumerator GrowScale()
    {
        Debug.Log("GrowScale" + " actualScale and targetScale "  + " " + actualScale + " " + targetScale);
        while (actualScale < targetScale)
        {
            Debug.Log("actualScale = " + actualScale + " localScale = " + transform.localScale);
            actualScale += actualScale * Time.deltaTime;
            transform.localScale = new Vector3(1,1,1) * actualScale;
            yield return null;
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
