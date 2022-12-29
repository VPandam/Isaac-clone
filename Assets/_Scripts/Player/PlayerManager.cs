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

    public int currentHealth, currentHealthContainers, currentBlueHealth; 
    [HideInInspector]  public GameObject currentTear;
    
    //Collectables
    [HideInInspector]
    public int currentBombs, currentKeys, currentCoins;
    
    //Stats
    [Header("Stats")]
    public int maxHealth;
    public float moveSpeed;
    public float fireRate;
    public float shotSpeed;
    public int attackDamage;
    
    [HideInInspector] public bool isInvincible;

    [Space(10)]
    
    //Starting stats
    [Header("Starting stats")] 
    public int startingBombs;
    public int startingKeys;
    public int startingCoins;
    public int startingHealthContainers;
    public int maxCollectables = 99;

    public delegate void OnUIChange();
    public OnUIChange onUIChangeCallback;


    [SerializeField] GameObject baseTear;
    Vector3 basicScaleBaseTear;
    Color basicColorBaseTear;
    
    [HideInInspector] public Room currentRoom;
   


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

        currentHealth = startingHealthContainers*2;
        currentBombs = startingBombs; 
        currentKeys = startingKeys; 
        currentCoins = startingCoins;
        currentHealthContainers = startingHealthContainers;
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
        {
            if (hpUpdate < 0 && currentHealth + hpUpdate < 0) currentHealth = 0;
            else currentHealth += hpUpdate;
        }
        
        else if (hpType == HpType.Blue)
        {
            if (hpUpdate < 0 && currentBlueHealth + hpUpdate < 0) currentBlueHealth = 0;
            else currentBlueHealth += hpUpdate;
        }

        if (onUIChangeCallback != null)
        {
            onUIChangeCallback.Invoke();
        }
    }    
    public void UpdateCoins(int coinsUpdate)
    {
        if (currentCoins <= maxCollectables)
        {
            currentCoins += coinsUpdate;
            if (onUIChangeCallback != null)
            {
                onUIChangeCallback.Invoke();
            }
        }

    }  public void UpdateBombs(int bombsUpdate)
    {
        if (currentBombs <= maxCollectables)
        {
            currentBombs += bombsUpdate;
            if (onUIChangeCallback != null)
            {
                onUIChangeCallback.Invoke();
            }
        }
    }  public void UpdateKeys(int keysUpdate)
    {
        if (currentKeys <= maxCollectables)
        {
            currentKeys += keysUpdate;
            if (onUIChangeCallback != null)
            {
                onUIChangeCallback.Invoke();
            }
        }

    }
    public IEnumerator InvincibilityOnHit(float time)
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreLayerCollision(10, 11, true);
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreLayerCollision(10, 11, false);
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

    public bool CheckIfWeCanGetMoreHp(int hpUpdate)
    {
        int totalPlayerHp = currentHealth + currentBlueHealth;
        //We cant have more health than the maximum health or more red health than the current health containers 
        if ((hpUpdate > 0) && ((totalPlayerHp >= maxHealth) || (currentHealth / 2 >= currentHealthContainers)))
        {
            return false;
        }
        else return true;
    }
    public bool CheckIfWeCanGetMoreBlueHp(int hpUpdate)
    {
        int totalPlayerHp = currentHealth + currentBlueHealth;
        //We cant have more health than the maximum health or more red health than the current health containers 
        if ((hpUpdate > 0) && (totalPlayerHp >= maxHealth))
        {
            return false;
        }
        else return true;
    }

}
    


