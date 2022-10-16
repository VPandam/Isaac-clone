using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HpType
{
    red, blue
}
public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerManager instance;
    public int currentHealth;
    public int currentBlueHealth;
    public int maxHealth;
    public int totalMaxHealth;
    public float moveSpeed;
    public float fireRate;
    public float shotSpeed;
    public float attackDamage;
    public GameObject currentTear;

    public delegate void OnHpChange(int hpChange);
    public OnHpChange OnHpChangeCallback;


    [SerializeField] GameObject baseTear;
    Vector3 basicScaleBaseTear;
    Color basicColorBaseTear;


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
    }

    private void OnApplicationQuit()
    {
        ResetTear();
    }
    void ResetTear()
    {
        baseTear.transform.localScale = basicScaleBaseTear;
        baseTear.GetComponent<SpriteRenderer>().color = basicColorBaseTear;
    }
    public void TakeDamage(int damage)
    {
        if (currentBlueHealth > 0)
        {
            int surplusDamage = damage - currentBlueHealth;
            UpdateHp(-damage, HpType.blue);

            if (surplusDamage > 0)
                UpdateHp(-surplusDamage, HpType.red);
        }
        else
        {
            UpdateHp(-damage, HpType.red);
        }
    }
    public void UpdateHp(int hpUpdate, HpType hpType)
    {
        if (hpType == HpType.red)
            currentHealth += hpUpdate;
            
        else if (hpType == HpType.blue)
            currentBlueHealth += hpUpdate;

        if (OnHpChangeCallback != null)
        {
            OnHpChangeCallback.Invoke(hpUpdate);
        }
    }

}
