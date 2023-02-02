using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum EnemyState
    {
        wander,
        dead,
        follow
    }

    public enum EnemyType
    {
        littleFly, chaser, worm, littleSpider
    }
    public class Enemy : MonoBehaviour, IDamageable, IExplodable

    {
        [HideInInspector] public GameObject player;
        [HideInInspector]public bool isKnockback, stopped;
        
        //Componetns
        public EnemyState _currentState;
        protected SpriteRenderer _spriteRenderer;
        protected Rigidbody2D _rb;
        [SerializeField]protected Sprite normalSprite;
        public Sprite _hitSprite;
        //Shared audio source plays the same sound for all the enemies of the same type while there is at least one alive.
        protected AudioSource _audioSource, sharedAudioSource;
        protected Animator _animator;
        private Room currentRoom;

        //Stats
        public int maxHp = 3, attackDamage = 1;
        public float rangeAttack = 1, speed = 1;
        [HideInInspector] protected int currentHp = 3;
        [SerializeField] protected LayerMask roomMask, playerMask;
        public EnemyType enemyType;

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

        //Clip played once independently of how many enemies there are.
        [SerializeField] private AudioClip sharedAudioClip;

        private void Awake()
        {
            resources = Resources.sharedInstance;
        }

        protected virtual void FixedUpdate()
        {
            if (isKnockback) return;
        }
        protected virtual void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerManager = player.GetComponent<PlayerManager>();
            currentHp = maxHp;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            normalSprite = _spriteRenderer.sprite;
            _rb = GetComponent<Rigidbody2D>();
            moveDirection = -transform.right;
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            currentRoom = GetComponentInParent<Room>();

            SetSharedAudioClip();
        }
        
        void SetSharedAudioClip()
        {
            foreach (var audioSourceEnemyType in currentRoom.audioSourceEnemyTypes)
            {
                if (audioSourceEnemyType.audioSource.clip == null && sharedAudioClip != null &&
                    audioSourceEnemyType.enemyType == enemyType)
                {
                    sharedAudioSource = audioSourceEnemyType.audioSource;
                    sharedAudioSource.clip = sharedAudioClip;
                }
            }
        }
        public void TakeDamage(int damage)
        {
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
            var enemies = currentRoom.enemies;
            enemies.Remove(this);
            _currentState = EnemyState.dead;

            //There are enemies that make a shared sound independently of how many there are.
            StopAudioSourceIfThereAreNotMoreEnemiesOfOurTypeAlive(enemies);

            if (enemies.Count == 0)
            {
                currentRoom.FinishRoom();
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// Stops the audioSource of our enemy type if there are not more enemies of our type alive.
        /// </summary>
        /// <param name="currentRoom"></param>
        /// <param name="enemies"></param>
        void StopAudioSourceIfThereAreNotMoreEnemiesOfOurTypeAlive(List<Enemy> enemies)
        {
            var audioSourceEnemyTypes = currentRoom.audioSourceEnemyTypes;

            if (!CheckIfThereAreEnemiesOfThisTypeAlive(enemies))
            {
                //Look for the audioSource of our enemy type.
                foreach (var audioSourceEnemyType in audioSourceEnemyTypes)
                {
                    if (audioSourceEnemyType.enemyType == enemyType)
                    {
                        audioSourceEnemyType.audioSource.Stop();
                    }
                }
            }
        }
        
        //Check if there are enemies of the same type of this class alive.
        //This is used to able or unable a shared sound.
        bool CheckIfThereAreEnemiesOfThisTypeAlive(List<Enemy> enemies)
        {
            bool ThereAreEnemiesOfThisTypeAlive = false;
            foreach (var enemy in enemies)
            {
                if (enemy.enemyType == enemyType) ThereAreEnemiesOfThisTypeAlive = true;
            }

            return ThereAreEnemiesOfThisTypeAlive;
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
        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawSphere(transform.position, raycastRange);
        //
        // }
    }
    





