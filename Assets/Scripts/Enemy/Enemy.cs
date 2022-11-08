using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    wander,
    dead,
    follow
}
public class Enemy : MonoBehaviour
{

    protected GameObject player;
    protected bool isKnockback;
    //Componetns
    public EnemyState _currentState;
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rb;

    protected Sprite normalSprite;
    public Sprite _hitSprite;

    //Stats
    protected float maxHp = 3f;
    protected float currentHp = 3f;
    protected int attackDamage = 1;
    protected float rangeAttack = 1f;
    protected float speed = 1f;

    protected LayerMask roomMask;
    protected LayerMask playerMask;

    //Movement
    protected float raycastRange = 1.5f;
    protected Vector2 moveDirection;
    protected bool choosingDirection = false;
    protected bool changingDirection = false;

    //OnCollisionStay
    protected float timer;
    protected float timeTick = 1;
    protected PlayerManager playerManager;



    private void Awake()
    {
        _currentState = EnemyState.wander;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = player.GetComponent<PlayerManager>();
        currentHp = maxHp;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = _spriteRenderer.sprite;
        _rb = GetComponent<Rigidbody2D>();
        moveDirection = -transform.right;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(isKnockback);
        if (isKnockback)
        {
            return;
        }
        switch (_currentState)
        {
            case EnemyState.wander:
                Wander();
                if (!changingDirection)
                    CheckCircle();

                break;
            // case enemyState.follow:
            //     follow();
            //     break;
            case EnemyState.dead:
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



    }
    /// <summary>
    /// Returns true if there is a collision
    /// </summary>
    /// <returns></returns>
    bool CheckCircle()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, raycastRange, roomMask);
        if (collider)
            StartCoroutine(ChangeDirection(collider));
        return collider != null;
    }
    // void CheckDirection()
    // {
    //     RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, moveDirection, raycastRange, roomMask);
    //     if (raycastHit)
    //         StartCoroutine(ChangeDirection(raycastHit));
    // }

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

    public virtual void Wander()
    {
        if (!choosingDirection)
        {
            StartCoroutine(ChooseDirection());
        }
        _rb.MovePosition(_rb.position + moveDirection * speed * Time.fixedDeltaTime);




        // if (IsPlayerInAttackRange() && currentState != enemyState.dead)
        // {
        //     currentState = enemyState.follow;
        // }
    }
    public virtual IEnumerator ChooseDirection()
    {
        choosingDirection = true;

        moveDirection = GetRandomDirection();
        FlipSprite();
        yield return new WaitForSeconds(Random.Range(5f, 10f));

        choosingDirection = false;

    }
    public virtual IEnumerator ChangeDirection(Collider2D collider)
    {
        changingDirection = true;
        moveDirection = -moveDirection;
        FlipSprite();
        yield return new WaitForSeconds(1f);
        changingDirection = false;
    }

    // void follow()
    // {
    //     this.speed = 2f;
    //     transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
    // }
    Vector2 GetRandomDirection()
    {
        float movementX = Random.Range(-100, 101);
        float movementY = Random.Range(-100, 101);
        return new Vector2(movementX, movementY).normalized;
    }
    void FlipSprite()
    {
        if (moveDirection.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
    public void TakeDamage()
    {
        currentHp -= PlayerManager.instance.attackDamage;
        StartCoroutine(BlinkColorDamage()); if (currentHp <= 0)
        {
            Room currentRoom = GetComponentInParent<Room>();
            var enemies = currentRoom.enemies;

            _currentState = EnemyState.dead;
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

            playerManager.TakeDamage(attackDamage);


        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer >= timeTick)
            {
                playerManager.TakeDamage(attackDamage);
                timer = 0;
            }
        }
    }
    public IEnumerator Knockback(Vector2 direction)
    {
        isKnockback = true;

        _rb.velocity = Vector2.zero;
        _rb.AddForce(direction, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);


        isKnockback = false;
    }
    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(transform.position, raycastRange);

    }
}
