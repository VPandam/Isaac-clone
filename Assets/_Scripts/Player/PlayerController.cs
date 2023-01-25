using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class PlayerController : MonoBehaviour
    {
        //Components
        Rigidbody2D _rb;
        [SerializeField]private Animator bodyAnimator;
        public static PlayerController _instance;
        CameraController _cam;
        private GameManager gameManager;
        
        [SerializeField]private GameObject BombPrefab;


        //Animations
        const string MOVING = "Moving";
        const string HORIZONTAL = "Horizontal";
        const string VERTICAL = "Vertical";
        const string LAST_MOVING_DIRECTION = "LastMoving";

        //The minimum input you need to apply to be captured
        public float sensibility = 0.3f;

        // Controller controls;
        public InputActions controls;
        Vector2 controlsMovement;


        Vector2 movementInput;
        bool moving;
        public bool isKnockback;

        RelocateAStarPath relocateAStarPath;



        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            gameManager = GameManager._instance;

            controls = new InputActions();
            controls.Enable();

            controls.Player.Bomb.performed += ctxt => PlaceBomb();
            controls.Player.OpenMinimap.performed += ctxt => Minimap._sharedInstance.OpenCloseMinimap();
            controls.Player.Fire.performed += context => Debug.Log(context.control.name);

            controls.Player.Move.canceled += ctxt =>
            {
                movementInput = Vector2.zero;
            };
            controls.Player.Pause.performed += ctxt =>
            {
                if (!gameManager.pause) gameManager.Pause();
                else gameManager.Resume();
            };



        }
        private void Start()
        {
            _cam = Camera.main.GetComponent<CameraController>();
            _rb = gameObject.GetComponent<Rigidbody2D>();
            relocateAStarPath = RelocateAStarPath.instance;
        }


        void Update()
        {
            if (GameManager._instance.pause || isKnockback)
            {
                bodyAnimator.SetBool(MOVING, false);
                return;
            }

            movementInput = controls.Player.Move.ReadValue<Vector2>();

            if (!moving)
            {
                bodyAnimator.SetBool(MOVING, false);
                _rb.velocity = Vector2.zero;
            }


        }
        private void FixedUpdate()
        {
            if (GameManager._instance.pause || isKnockback)
            {
                bodyAnimator.SetBool(MOVING, false);
                return;
            }

            moving = movementInput.normalized.magnitude > sensibility;

            if (moving)
                Move();
        }
        void Move()
        {
            float movementSpeed = PlayerManager.sharedInstance.moveSpeed;
            bodyAnimator.SetFloat(HORIZONTAL, movementInput.x);
            bodyAnimator.SetFloat(VERTICAL, movementInput.y);
            bodyAnimator.SetBool(MOVING, true);
            _rb.MovePosition(_rb.position + movementInput * (movementSpeed * Time.fixedDeltaTime));
            CardinalDirection lastMovingDirection = Resources.sharedInstance.GetCardinalDirection(movementInput);
            bodyAnimator.SetInteger(LAST_MOVING_DIRECTION, (int)lastMovingDirection);
        }


        /// <summary>
        /// When crossing a door, move the camera to the new room.
        /// Change the player position to the new room player spawn. 
        /// Destroy all bullets
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DoorExitZone"))
            {
                ExitZone exitZone = collision.GetComponent<ExitZone>();
                Room roomToSpawn = exitZone.roomToSpawn;
                ChangeRoom(roomToSpawn, exitZone);
            }
        }
        public void ChangeRoom(Room roomToSpawn,  ExitZone exitZone = null)
        {
            Room currentRoom = PlayerManager.sharedInstance.currentRoom;
            if (roomToSpawn)
            {
                currentRoom.OnLeftRoom();
                PlayerManager.sharedInstance.currentRoom = roomToSpawn;
                roomToSpawn.SetVisibleOnMinimap();
                //Fade in black screen
                if(exitZone) GameManager._instance.StartCoroutine("FadeInFadeOut");

                //Destroy all bullets
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
                foreach (var bullet in bullets)
                {
                    Destroy(bullet);
                }

                //Move the camera to the new room
                _cam.MoveCameraTo(roomToSpawn.transform.position);
                if (!roomToSpawn.playerEntered)
                {
                    roomToSpawn.playerEntered = true;
                    roomToSpawn.Invoke("StartRoom", .6f);
                }

                if (exitZone)
                {
                    //Move the player to the new room spawn position
                    gameObject.transform.position = exitZone.playerSpawnPosition;
                }
                else
                {
                    //If we are not tping from a spawn room, we are starting a new level.
                    gameObject.transform.position = Vector3.zero;
                }
                relocateAStarPath.Relocate(roomToSpawn.transform.position);
            }
        }

        void PlaceBomb()
        {
            PlayerManager playerManager = PlayerManager.sharedInstance;
            if (playerManager.currentBombs >= 1)
            { 
                playerManager.currentBombs--;
                if (playerManager.onUIChangeCallback != null)
                {
                    playerManager.onUIChangeCallback.Invoke();
                }
                Instantiate(BombPrefab, transform.position, Quaternion.identity);
            }
        }
        public IEnumerator Knockback(Vector2 direction)
        {
            isKnockback = true;

            _rb.velocity = direction;
            // _rb.AddForce(direction, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.2f);

            isKnockback = false;
        }
        private void OnDisable()
        {
            controls.Disable();
        }

    }




