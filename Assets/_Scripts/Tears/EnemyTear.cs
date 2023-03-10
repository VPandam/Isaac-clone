using UnityEngine;


    public class EnemyTear : Tear
    {
        private Enemy _enemy;
        int _attackDamage;
        private float range;
        
        public  void SetEnemyBullet(Vector2 shotDirection, float speed, int attackDamage, float range)
        {
            _shotDirection = shotDirection;
            _speed = speed;
            _attackDamage = attackDamage;
            this.range = range;
            Invoke("DestroyTear", this.range);

        }
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals("Player"))
            {
                PlayerManager playerManager = other.GetComponent<PlayerManager>();
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerManager)
                {
                    playerManager.TakeDamage(_attackDamage);
                    if (!playerController.isKnockback)
                        playerController.StartCoroutine(playerController.Knockback(
                            (playerManager.transform.position - transform.position).normalized * 1));
                }
                Destroy(gameObject);
            }

        }
    }


