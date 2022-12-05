using UnityEngine;


    public class EnemyBullet : Bullet
    {
        int _attackDamage;
        public void SetBullet(Vector2 shotDirection, float speed, int attackDamage)
        {
            _shotDirection = shotDirection;
            _speed = speed;
            _attackDamage = attackDamage;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag.Equals("Player") || other.tag.Equals("Wall"))
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


