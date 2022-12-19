using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IExplodable
{
    [SerializeField] GameObject explosionParticles;
    private GameObject particleInstance;
    public void Explode(int damage)
    {
        particleInstance = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Invoke(nameof(DestroyParticles), 1.5f);
        Destroy(gameObject);
    }

    void DestroyParticles()
    {
        Destroy(particleInstance);
    }
}
