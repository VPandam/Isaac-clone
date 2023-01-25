using Pathfinding;
using UnityEngine;


public class EnemyIA : MonoBehaviour
    {
        public Transform target;
        public float speed = 3;
        public float nextWaypointDistance = 2f;

        private Path path;
        private Rigidbody2D _rb;
        private int currentWaypoint = 0;
        private Seeker _seeker;

        private Enemy _enemy;

        private Vector2 direction;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _seeker = GetComponent<Seeker>();
            _enemy = GetComponent<Enemy>();

            if (_enemy)
            {
                target = _enemy.player.transform;
                _seeker.StartPath(transform.position, target.position, OnPathComplete);
                InvokeRepeating("UpdatePath", 0f, .5f);
            } 
        }

        void UpdatePath()
        {
            if(_seeker.IsDone())
                _seeker.StartPath(transform.position, target.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }

        private void Update()
        {
            if (path == null) return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                return;
            }


            direction = ((Vector2)(path.vectorPath[currentWaypoint] - transform.position)).normalized;
            
          
            transform.Translate((Vector3)direction * (Time.deltaTime * speed));
          

            float distance = Vector2.Distance((Vector2)transform.position ,(Vector2)path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }

        private void FixedUpdate()
        {
            // if(_rb) _rb.MovePosition(_rb.position + direction * (Time.fixedDeltaTime * speed));
        }
        
        
    }

