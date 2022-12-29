using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

enum PoopBossState
{
    Shoot, Tackle, Chill
}

public class PoopBoss : Enemy
{
    private PoopBossState currentPoopBossState;

    private bool changingCurrentState, shooting, tackleing;
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private float bulletSpeed, angleBetweenBullets;
    [SerializeField] private int numberOfBullets;
    float shootAngle;

    [Header("Tackleing")] 
    [SerializeField] private float tackleSpeed;
    [SerializeField] float tackleTime;

    protected override void Start()
    {
        base.Start();
        currentPoopBossState = PoopBossState.Chill;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentPoopBossState)
        {
            case PoopBossState.Chill:
                if(!changingCurrentState)StartCoroutine(nameof(ChangeToRandomState));
                break;
            case PoopBossState.Shoot:
                if(!shooting)StartCoroutine(nameof(Shoot));
                break;
            case PoopBossState.Tackle:
                if(!tackleing)StartCoroutine(nameof(Tackle));
                break;
        }
    }

    IEnumerator ChangeToRandomState()
    {
        changingCurrentState = true;
        float randomTime = Random.Range(2,3);
        yield return new WaitForSeconds(randomTime);
        //0 is Shoot, 1 is tackle
        // int randomIndex = Random.Range(0, 2);
        int randomIndex = 1;
        if (randomIndex == 0)currentPoopBossState = PoopBossState.Shoot;
        else if (randomIndex == 1)currentPoopBossState = PoopBossState.Tackle;
        changingCurrentState = false;
    } 
    
    /// <summary>
    ///Shots a number of bullets in a cone with the angleBetweenBullets.
    ///The second bullet will always face the player.
    /// </summary>
    IEnumerator Shoot()
        {
            shooting = true;
            yield return new WaitForSeconds(1);
            float playerRotation = CalculatePlayerRotation();
            //Im aiming to shot 3 bullets, so we add the angle between bullets to the first one.
            //After that we will substract it everytime we shoot.
            shootAngle = playerRotation + angleBetweenBullets;
            for (var i = 0; i < numberOfBullets; i++)
            {
                InstantiateTear();
                shootAngle -= angleBetweenBullets;
            };
            currentPoopBossState = PoopBossState.Chill;
            shooting = false;
        }

    void InstantiateTear()
    {
        var rotation = Quaternion.AngleAxis(shootAngle, Vector3.forward);
        GameObject tear = Instantiate(bulletPref, transform.position, rotation);
        tear.GetComponent<EnemyTear>().SetEnemyBullet(tear.transform.right, bulletSpeed, attackDamage);
    }

    float CalculatePlayerRotation()
    {
        var dir = player.transform.position - transform.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    IEnumerator Tackle()
    {
        tackleing = true;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Vector3 startingPos  = transform.position;
            Vector3 finalPos = player.transform.position;
            float timer = 0;
            while (timer <= tackleTime)
            {
                Vector2 direction = finalPos - startingPos;
                _rb.velocity = (direction.normalized * tackleSpeed);
                timer += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            _rb.velocity = Vector2.zero;
            
            // float elapsedTime = 0;
             
            // while (elapsedTime < tackleSpeed)
            // {
            //     transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / tackleSpeed));
            //     elapsedTime += Time.deltaTime;
            //     yield return null;
            // }
        }
        currentPoopBossState = PoopBossState.Chill;
        tackleing = false;
    }
}
