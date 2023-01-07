using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Tear : MonoBehaviour
    {
        public float _speed;
        GameObject player;
        GameObject enemy;
        Rigidbody2D _rb;
        [HideInInspector]public Vector2 _shotDirection; 
        [HideInInspector]public AudioSource playerAudioSource;
        public AudioClip tearCollidesSound, tearCollidesEnemySound;
        public AudioClip[] tearSounds;
        [SerializeField]protected GameObject colliderWithWall;
        [SerializeField] private ParticleSystem _particleSystem;

        public virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerAudioSource = PlayerManager.sharedInstance.playerAudioSource;
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            int index = Random.Range(0, tearSounds.Length);
            if (tearSounds.Length > 0)
                playerAudioSource.PlayOneShot(tearSounds[index]);
            // _rb.velocity = shotDirection * speed;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _shotDirection * (Time.fixedDeltaTime * _speed));
        }

        public virtual void SetBullet(Vector2 shotDirection, float speed)
        {
            _shotDirection = shotDirection;
            _speed = speed;
        }
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy)
                {
                    //We are hitting an enemy
                    if (tearCollidesEnemySound)
                        playerAudioSource.PlayOneShot(tearCollidesEnemySound, 0.2f);
                    enemy.TakeDamage(PlayerManager.sharedInstance.attackDamage);
                    if (!enemy.isKnockback)
                        enemy.StartCoroutine(enemy.Knockback(
                            (enemy.transform.position - transform.position).normalized * 1));
                    Destroy(gameObject);
                }
            }
        }

        void DestroyTear()
        {
            if(_particleSystem) Instantiate(_particleSystem, transform.position, _particleSystem.transform.rotation);
            Destroy(gameObject);
        }


    }

