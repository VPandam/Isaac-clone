using System.Collections;
using UnityEngine;


    public enum EnemyState
    {
        wander,
        dead,
        follow
    }


    public class Enemy : MonoBehaviour, IDamageable, IExplodable

    {
        [HideInInspector] public GameObject player;
        [HideInInspector]public bool isKnockback;
        //Componetns
        public EnemyState _currentState;
        protected SpriteRenderer _spriteRenderer;
        protected Rigidbody2D _rb;
        [SerializeField]protected Sprite normalSprite;
        public Sprite _hitSprite;
        protected AudioSource _audioSource;

        //Stats
        [SerializeField] protected int maxHp = 3,  rangeAttack = 1, attackDamage = 1, speed = 1;
        [HideInInspector] protected int currentHp = 3;
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

        protected Resources resources;

        private void Awake()
        {
            resources = Resources.sharedInstance;
        }

        private void FixedUpdate()
        {
            if (isKnockback)
            {
                return;
            }
        }
        protected virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                Debug.Log(player.gameObject.name + " Hay player o que");
            else
            {
                Debug.Log("Player = null");
            }
            playerManager = player.GetComponent<PlayerManager>();
            currentHp = maxHp;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            normalSprite = _spriteRenderer.sprite;
            _rb = GetComponent<Rigidbody2D>();
            moveDirection = -transform.right;
            _audioSource = GetComponent<AudioSource>();
        }


        public void TakeDamage(int damage)
        {
            Debug.Log("TakeDamage");
            currentHp -= damage;
            StartCoroutine(BlinkColorDamage());
            if (currentHp <= 0)
            {
                Die();
            }
        }

        public void Explode(int damage)
        {
            TakeDamage(damage);
        }
        void Die()
        {
            Room currentRoom = GetComponentInParent<Room>();
            var enemies = currentRoom.enemies;

            enemies.Remove(this);
            _currentState = EnemyState.dead;
            if (enemies.Count == 0)
            {
                currentRoom.FinishRoom();
            }
            Destroy(gameObject);
        }

        protected virtual IEnumerator BlinkColorDamage()
        {
            _spriteRenderer.sprite = _hitSprite;
            yield return new WaitForSeconds(0.07f);
            _spriteRenderer.sprite = normalSprite;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" && !playerManager.isInvincible)
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
            if (_rb)
            {
                _rb.velocity = Vector2.zero;
                _rb.AddForce(direction * 5, ForceMode2D.Impulse);
                _rb.velocity = Vector2.zero;
            }
            yield return new WaitForSeconds(.2f);


            isKnockback = false;
        }
        private void OnDrawGizmos()
        {
            // Gizmos.color = Color.red;
            // Gizmos.DrawSphere(transform.position, raycastRange);

        }
    }
    





