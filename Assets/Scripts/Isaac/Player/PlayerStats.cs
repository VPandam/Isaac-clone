using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerStats instance;
    public StatsScrO playerStatsScrO;
    private int currentHealth;
    private int maxHealth;
    private float moveSpeed;
    private float fireRate;
    private float shotSpeed;
    private float attackDamage;

    public delegate void OnHpChange(int hpChange);
    public OnHpChange OnHpChangeCallback;

    GameObject currentTear;

    [SerializeField]
    GameObject baseTear;
    Vector3 basicScaleBaseTear;
    Color basicColorBaseTear;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
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

        GetStatsFromSO();

        if (currentTear == null)
            currentTear = baseTear;


        basicColorBaseTear = baseTear.GetComponent<SpriteRenderer>().color;
        basicScaleBaseTear = baseTear.transform.localScale;

        currentHealth = maxHealth;

    }
    void GetStatsFromSO()
    {
        maxHealth = playerStatsScrO.maxHealth;
        attackDamage = playerStatsScrO.attackDamage;
        fireRate = playerStatsScrO.fireRate;
        moveSpeed = playerStatsScrO.moveSpeed;
        shotSpeed = playerStatsScrO.shotSpeed;

    }

    private void OnApplicationQuit()
    {
        baseTear.transform.localScale = basicScaleBaseTear;
        baseTear.GetComponent<SpriteRenderer>().color = basicColorBaseTear;
    }

    public void UpdateHp(int hpUpdate)
    {
        CurrentHealth -= hpUpdate;
        if (OnHpChangeCallback != null)
        {
            OnHpChangeCallback.Invoke(hpUpdate);
        }
    }

}
