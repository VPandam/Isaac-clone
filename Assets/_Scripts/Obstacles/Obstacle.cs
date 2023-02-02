using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IExplodable
{
    [SerializeField]private GameObject onExplodeParticleGO;

    public virtual void Explode(int damage)
    {
        if(onExplodeParticleGO) onExplodeParticleGO = Instantiate(onExplodeParticleGO, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        RelocateAStarPath relocateAStarPath = RelocateAStarPath.instance;
        if(relocateAStarPath)relocateAStarPath.StartCoroutine(nameof(relocateAStarPath.GridScan));
    }
}
