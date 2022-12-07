using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public enum HpType
{
    Red, Blue
}
public class PlayerManager : MonoBehaviour, IDamageable, IExplodable
{
    public static PlayerManager sharedInstance;

    SpriteRenderer _spriteR;
    [HideInInspector] public AudioSource playerAudioSource;

    //Stats
    public int currentHealth;
    public int currentHealthContainers;
    public int currentBlueHealth;
    public int maxHealth;
    public int totalMaxHealth;
    public float moveSpeed;
    public float fireRate;
    public float shotSpeed;
    public int attackDamage;
    public GameObject currentTear;
    public bool isInvincible;

    //Collectables
    public int currentBombs, currentKeys, currentCoins;
    public int StartingBombs, StartingKeys, StartingCoins, maxCollectables = 99;

    public delegate void OnUIChange();
    public OnUIChange onUIChangeCallback;


    [SerializeField] GameObject baseTear;
    Vector3 basicScaleBaseTear;
    Color basicColorBaseTear;
   


    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;


    }

    private void Start()
    {
        _spriteR = GetComponent<SpriteRenderer>();
        playerAudioSource = GetComponent<AudioSource>();
        
        if (currentTear == null)
            currentTear = baseTear;


        basicColorBaseTear = baseTear.GetComponent<SpriteRenderer>().color;
        basicScaleBaseTear = baseTear.transform.localScale;

        currentHealth = maxHealth;

        currentBombs = StartingBombs; 
        currentKeys = StartingKeys; 
        currentCoins = StartingCoins;
        onUIChangeCallback.Invoke();
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
                UpdateHp(-damage, HpType.Blue);

                if (surplusDamage > 0)
                    UpdateHp(-surplusDamage, HpType.Red);
            }
            else
            {
                UpdateHp(-damage, HpType.Red);
            }
            // StartCoroutine(StopMovingAfterDamage());
        }
    }

    public void Explode(int damage)
    {
      TakeDamage(damage);
    }

    public IEnumerator StopMovingAfterDamage()
    {
        GameManager._instance.Pause();
        yield return new WaitForSeconds(0.2f);
        GameManager._instance.Resume();
    }

    public void UpdateHp(int hpUpdate, HpType hpType)
    {
        if (hpType == HpType.Red)
            currentHealth += hpUpdate;

        else if (hpType == HpType.Blue)
            currentBlueHealth += hpUpdate;

        if (onUIChangeCallback != null)
        {
            onUIChangeCallback.Invoke();
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
    


