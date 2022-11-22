using System.Collections;
using UnityEngine;


public enum HpType
{
    red, blue
}
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager sharedInstance;

    [SerializeField] SpriteRenderer _spriteR;

    //Stats
    public int currentHealth;
    public int currentBlueHealth;
    public int maxHealth;
    public int totalMaxHealth;
    public float moveSpeed;
    public float fireRate;
    public float shotSpeed;
    public float attackDamage;
    public GameObject currentTear;
    public bool isInvincible;

    public delegate void OnHpChange(int hpChange);
    public OnHpChange OnHpChangeCallback;


    [SerializeField] GameObject baseTear;
    Vector3 basicScaleBaseTear;
    Color basicColorBaseTear;


    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;


        if (currentTear == null)
            currentTear = baseTear;


        basicColorBaseTear = baseTear.GetComponent<SpriteRenderer>().color;
        basicScaleBaseTear = baseTear.transform.localScale;

        currentHealth = maxHealth;

        _spriteR = GetComponent<SpriteRenderer>();

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
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityOnHit(0.7f));
            StartCoroutine(FlashOnInvincibility());
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
            // StartCoroutine(StopMovingAfterDamage());
        }
    }
    public IEnumerator StopMovingAfterDamage()
    {
        GameManager._instance.Pause();
        yield return new WaitForSeconds(0.2f);
        GameManager._instance.Resume();
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
    public IEnumerator InvincibilityOnHit(float time)
    {
        isInvincible = true;
        yield return new WaitForSeconds(time);
        isInvincible = false;
    }
    public IEnumerator FlashOnInvincibility()
    {
        while (isInvincible)
        {
            _spriteR.enabled = false;
            yield return new WaitForSeconds(0.05f);
            _spriteR.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

}
    


