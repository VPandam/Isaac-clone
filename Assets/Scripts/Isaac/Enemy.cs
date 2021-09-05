using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum enemyState{
    wander,
    dead,
    follow
}
public class Enemy : MonoBehaviour
{


    enemyState currentState;
    Transform enemyTransform;

    public int currentHp;

    public float rangeAttack = 1f;
    public float speed = 1f;
    public LayerMask roomMask;
    Quaternion nextRotation;

    float angle;
    public float changingDirectionSpeed;

    Vector3 randomDirection;
    Vector3 moveDirection;
    

    bool choosingDirection = false;
    bool changingDirection = false;

    GameObject player;

    float initialSpeed;
    // Start is called before the first frame update
    void Start()
    {
        currentState = enemyState.wander;
        enemyTransform = gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        currentHp = 3;
    }
    private void Awake()
    {
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case enemyState.wander:
                wander();
                if (Physics2D.Raycast(enemyTransform.position, -transform.right, 2f, roomMask) && changingDirection != true)
                {
                    StartCoroutine("ChangeDirection");
                }
                break;
            case enemyState.follow:
                follow();
                break;
            case enemyState.dead:
                Destroy(gameObject);
                break;

        }

        if (!IsPlayerInAttackRange() && currentState != enemyState.dead)
        {
            currentState = enemyState.wander;
        }else if (IsPlayerInAttackRange() && currentState != enemyState.dead)
        {
            currentState = enemyState.follow;
        }

        Debug.DrawRay(enemyTransform.position, -transform.right *2f, Color.red);

      
    }
    bool IsPlayerInAttackRange()
    {
        if (Vector3.Distance(enemyTransform.position, player.transform.position) <= rangeAttack)
        {
            return true;
        }
        else
        {
            return false;
        };
    }

    void wander()
    {
        this.speed = initialSpeed;
        if (!choosingDirection)
        {
            StartCoroutine("ChooseDirection");
        }
        else {
            enemyTransform.position += -enemyTransform.right * speed * Time.deltaTime;
            
        }

        if (IsPlayerInAttackRange() && currentState != enemyState.dead)
        {
            currentState = enemyState.follow;

        }
    }
    IEnumerator ChooseDirection()
    {
        choosingDirection = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        angle = Random.Range(0, 360);
        randomDirection = new Vector3(0, 0, angle);
        nextRotation = Quaternion.Euler(randomDirection);
        enemyTransform.rotation = Quaternion.Lerp(enemyTransform.rotation, nextRotation, Time.deltaTime * Random.Range(0.5f, 2.5f));
        choosingDirection = false;

    }
    IEnumerator ChangeDirection()
    {
        changingDirection = true;
        Debug.Log("Changing direction");
        angle = Random.Range(0,360);
        randomDirection = new Vector3(0, 0, angle);
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        enemyTransform.rotation = Quaternion.Lerp(enemyTransform.rotation, nextRotation, 1);
        yield return new WaitForSeconds(1f); 
        changingDirection = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeDirection();
    }
    void follow()
    {
        this.speed = 2f;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        
    }
    
    public void CollectHp (int hp)
    {
        currentHp += hp;
        if (currentHp <= 0)
        {
            var enemies = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>().enemies;
            enemies.Remove(this);
            if (enemies.Count == 0)
            {
                GameObject.FindGameObjectWithTag("Room").GetComponent<Room>().OpenRoom();
            }
            currentState = enemyState.dead;
        }
    }
    void OpenRoom()
    {
        GameObject[] enemiesRest = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("finGameObjectsWithTagEnemy" + enemiesRest.Length + enemiesRest.GetValue(0));

        if (enemiesRest.Length == 0)
        {
            
            GameObject.FindGameObjectWithTag("Room").GetComponent<Room>().OpenRoom();
        }
    }

}
