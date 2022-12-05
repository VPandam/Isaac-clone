using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IExplodable
{
    public void Explode(int damage)
    {
        Destroy(gameObject);
    }
}
