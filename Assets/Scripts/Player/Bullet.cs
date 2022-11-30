using UnityEngine;



    public class Bullet : MonoBehaviour
    {
        public float _speed;
        GameObject player;
        GameObject enemy;
        Rigidbody2D _rb;
        public Vector2 _shotDirection;
        AudioSource playerAudioSource;
        public AudioClip tearCollidesSound, tearCollidesEnemySound;
        public AudioClip[] tearSounds;

        void Start()
        {
            Destroy(gameObject, 5f);
            _rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerAudioSource = player.GetComponent<Shooting>()._audioSource;
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            int index = Random.Range(0, tearSounds.Length);
            Debug.Log(index);
            if (tearSounds.Length > 0)
                playerAudioSource.PlayOneShot(tearSounds[index]);
            // _rb.velocity = shotDirection * speed;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _shotDirection * (Time.fixedDeltaTime * _speed));
        }

        public void SetBullet(Vector2 shotDirection, float speed)
        {
            _shotDirection = shotDirection;
            _speed = speed;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("OnTriger");
            if (other.tag.Equals("Enemy") || other.tag.Equals("Wall"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy)
                {
                    if (tearCollidesEnemySound)
                        playerAudioSource.PlayOneShot(tearCollidesEnemySound, 0.2f);
                    enemy.TakeDamage();
                    if (!enemy.isKnockback)
                        enemy.StartCoroutine(enemy.Knockback(
                            (enemy.transform.position - transform.position).normalized * 1));
                }
                else
                {
                    if (tearCollidesSound)
                        playerAudioSource.PlayOneShot(tearCollidesSound, 0.2f);
                }

                Destroy(gameObject);
            }
        }
    }

