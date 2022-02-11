using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerStats instance;

    private int health = 10;
    private int maxHealth = 10;
    private float moveSpeed = 4f;
    private float fireRate = 0.5f;
    private float shotSpeed = 3f;
    private float attackDamage = 3f;

    GameObject currentTear;
    public GameObject baseTear;

    public int Health { get => health; set => health = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float ShotSpeed { get => shotSpeed; set => shotSpeed = value; }
    public GameObject CurrentTear { get => currentTear; set => currentTear = value; }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (currentTear == null)
            currentTear = baseTear;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
