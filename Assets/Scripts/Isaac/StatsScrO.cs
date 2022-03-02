using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "statsScrO", menuName = "StatsSO")]
public class StatsScrO : ScriptableObject
{
    // Start is called before the first frame update
    public int maxHealth;
    public float moveSpeed;
    public float fireRate;
    public float shotSpeed;
    public float attackDamage;
}

