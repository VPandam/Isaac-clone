using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum enemyState
{
    wander,
    dead,
    follow
}
public class Enemy : MonoBehaviour
{

    GameObject player;

    //Componetns
    enemyState _currentState;
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rb;

    Sprite normalSprite;
    public Sprite _hitSprite;

    //Stats
    float maxHp = 3f;
    float currentHp = 3f;
    int attackDamage = 1;
    public float rangeAttack = 1f;
    public float speed = 1f;

    public LayerMask roomMask;

    //Movement
    Quaternion nextRotation;
    float angle;
    public float changingDirectionSpeed;
    Vector3 randomDirection;
    Vector3 moveDirection;
    bool choosingDirection = false;
    bool changingDirection = false;


    float initialSpeed;


    // Start is called before the first frame update
    void Start()
    {
        _currentState = enemyState.wander;
        player = GameObject.FindGameObjectWithTag("Player");
        currentHp = maxHp;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = _spriteRenderer.sprite;
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        initialSpeed = speed;
        _currentState = enemyState.wander;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        switch (_currentState)
        {
            case enemyState.wander:
                wander();
                // if (Physics2D.Raycast(transform.position, -transform.right, 2f, roomMask) && changingDirection != true)
                // {
                //     StartCoroutine("ChangeDirection");
                // }
                break;
            // case enemyState.follow:
            //     follow();
            //     break;
            case enemyState.dead:
                Destroy(gameObject);
                break;

        }

        // if (!IsPlayerInAttackRange() && currentState != enemyState.dead)
        // {
        //     currentState = enemyState.wander;
        // }
        // else if (IsPlayerInAttackRange() && currentState != enemyState.dead)
        // {
        //     currentState = enemyState.follow;
        // }

        Debug.DrawRay(transform.position, -transform.right * 2f, Color.red);


    }
    bool IsPlayerInAttackRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= rangeAttack)
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
        // if (!choosingDirection)
        // {
        //     StartCoroutine(ChooseDirection());
        // }
        // else

        Vector2 right = new Vector2(transform.right.x, transform.right.y);
        _rb.MovePosition(_rb.position + -right * speed * Time.fixedDeltaTime);


        // if (IsPlayerInAttackRange() && currentState != enemyState.dead)
        // {
        //     currentState = enemyState.follow;
        // }
    }
    IEnumerator ChooseDirection()
    {
        choosingDirection = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        angle = Random.Range(0, 360);
        randomDirection = new Vector3(0, 0, angle);
        nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.fixedDeltaTime * Random.Range(0.5f, 2.5f));
        choosingDirection = false;

    }
    IEnumerator ChangeDirection()
    {
        changingDirection = true;
        Debug.Log("Changing direction");
        angle = Random.Range(0, 360);
        randomDirection = new Vector3(0, 0, angle);
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, 1);
        yield return new WaitForSeconds(1f);
        changingDirection = false;
    }

    // void follow()
    // {
    //     this.speed = 2f;
    //     transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
    // }

    public void TakeDamage()
    {
        currentHp -= PlayerManager.instance.attackDamage;
        StartCoroutine(BlinkColorDamage());
        if (currentHp <= 0)
        {
            Room currentRoom = GetComponentInParent<Room>();
            var enemies = currentRoom.enemies;

            _currentState = enemyState.dead;
            enemies.Remove(this);
            if (enemies.Count == 0)
            {
                currentRoom.OpenRoom();
            }
        }
    }
    IEnumerator BlinkColorDamage()
    {
        _spriteRenderer.sprite = _hitSprite;
        yield return new WaitForSeconds(0.07f);
        _spriteRenderer.sprite = normalSprite;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("PlayerCollision");
            // Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            // playerRB.AddForce((collision.transform.position - transform.position).normalized,
            // ForceMode2D.Impulse);

            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(attackDamage);


        }
    }

}
