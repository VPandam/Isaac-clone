using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    wander,
    dead,
    follow
}
public enum CardinalDirection
{
    up, down, left, right
}
// public struct CardinalDirection
// {

// }
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
    [SerializeField] protected float maxHp = 3f, currentHp = 3f, rangeAttack = 1f;
    [SerializeField] protected int attackDamage = 1, speed = 1;

    [SerializeField] protected LayerMask roomMask, playerMask;

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

    public void TakeDamage()
    {
        Debug.Log("TakeDamage");
        currentHp -= PlayerManager.sharedInstance.attackDamage;
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
