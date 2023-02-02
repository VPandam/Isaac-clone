using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public enum HpType
{
    Red, Blue
}

public class PlayerManager : MonoBehaviour, IDamageable, IExplodable
{
    public static PlayerManager sharedInstance;

    [SerializeField] private GameObject headGO, bodyGO;
    private Animator playerGeneralAnimator;
    private SpriteRenderer headSR;

    [SerializeField]private AudioClip[] onHitAudioClips;
        
    [HideInInspector] public AudioSource playerAudioSource;

    [HideInInspector]public int currentHealth, currentHealthContainers, currentBlueHealth;
    [HideInInspector] public GameObject currentTear;

    //Collectables
    [HideInInspector] public int currentBombs, currentKeys, currentCoins;

    //Stats
    [Header("Stats")] public int maxHealth;
    public float moveSpeed, shotSpeed, attackRange;

    [SerializeField]private float attackSpeed;
    //1 is the maximum time we will wait between shots.
    public float AttackSpeed => 1f / attackSpeed;

    public int attackDamage;


    [HideInInspector] public bool isInvincible, dead;

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
        dead = false;
        headSR = headGO.GetComponent<SpriteRenderer>();
        playerAudioSource = GetComponent<AudioSource>();
        playerGeneralAnimator = GetComponent<Animator>();

        if (currentTear == null)
            currentTear = baseTear;


        basicColorBaseTear = baseTear.GetComponent<SpriteRenderer>().color;
        basicScaleBaseTear = baseTear.transform.localScale;

        currentHealth = startingHealthContainers * 2;
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
            playerAudioSource.PlayOneShot(onHitAudioClips[Random.Range(0, onHitAudioClips.Length)], .5f);
            StartCoroutine(InvincibilityOnHit(0.7f));
            StartCoroutine(FlashOnInvincibility());
            if (currentBlueHealth > 0)
            {
                int surplusDamage = damage - currentBlueHealth;
                UpdateHp(-damage, HpType.Blue);

                if (surplusDamage > 0){}
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
        GameManager._instance.PauseMenu();
        yield return new WaitForSeconds(0.2f);
        GameManager._instance.Resume();
    }

    public void UpdateHp(int hpUpdate, HpType hpType)
    {
        if (hpType == HpType.Red)
        {
            int hpAfterUpdate = currentHealth + hpUpdate;
            //Check if hpUpdate is negative and if we will be having negative hp after update.
            if (hpUpdate < 0 &&  hpAfterUpdate <= 0)  Die();
            //We can't have more red health than the current health containers, if the hp after update is bigger than the max hp, set it to max. 
            else if (hpUpdate > 0 && hpAfterUpdate > currentHealthContainers * 2)
                currentHealth = currentHealthContainers * 2;
            else currentHealth += hpUpdate;
        }

        else if (hpType == HpType.Blue)
        {
            //Check if blueHpUpdate is negative and if we will be having negative blue hp after update.
            if (hpUpdate < 0 && currentBlueHealth + hpUpdate < 0) currentBlueHealth = 0;
            else currentBlueHealth += hpUpdate;
        }

        if (onUIChangeCallback != null)
        {
            onUIChangeCallback.Invoke();
        }
    }

    void Die()
    {
        dead = true;
        DisableHeadAndBodyRenderers();
        GetComponent<Collider2D>().enabled = false;
        playerGeneralAnimator.SetBool("Dead", true);
        GameManager._instance.Invoke(nameof(Die), 2);
    }

    //Head and body have different animations.
    //There are some animations that are full body, we need to disable head and body SR to run those.
    void DisableHeadAndBodyRenderers()
    {
        headGO.GetComponent<SpriteRenderer>().enabled = false;
        bodyGO.GetComponent<SpriteRenderer>().enabled = false;
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

    }
    public void UpdateBombs(int bombsUpdate)
    {
        if (currentBombs <= maxCollectables)
        {
            currentBombs += bombsUpdate;
            if (onUIChangeCallback != null)
            {
                onUIChangeCallback.Invoke();
            }
        }
    }
    public void UpdateKeys(int keysUpdate)
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
        //Ignore collisions with enemies for a short time
        //Layer 10 = Player, 11 = Enemy, 14 = FlyingEnemy
        Physics2D.IgnoreLayerCollision(10, 11, true);
        Physics2D.IgnoreLayerCollision(10, 14, true);
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreLayerCollision(10, 11, false);
        Physics2D.IgnoreLayerCollision(10, 14, false);
        isInvincible = false;
    }
    public IEnumerator FlashOnInvincibility()
    {

        while (isInvincible && !dead)
        {
            headSR.enabled = false;
            yield return new WaitForSeconds(0.05f);
            if(!dead) headSR.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }
    

    public bool CheckIfWeCanGetMoreHp()
    {
    int totalPlayerHp = currentHealth + currentBlueHealth;

    //We cant have more health than the maximum health
    if (totalPlayerHp >= maxHealth || currentHealth >= currentHealthContainers*2)
    {
        return false;
    }
    return true;
        
    }
    public bool CheckIfWeCanGetMoreBlueHp()
    {
        int totalPlayerHp = currentHealth + currentBlueHealth;
        //We cant have more health than the maximum health
        if (totalPlayerHp >= maxHealth)
        {
            return false;
        }
        else return true;
    }
    
    public void SetAttackSpeed(float attSpeedAmount)
    {
        attackSpeed += attSpeedAmount;
        attackSpeed = Mathf.Clamp(attackSpeed, 1, 3.5f);
    }

}



