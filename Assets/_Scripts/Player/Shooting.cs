using System.Collections;
using UnityEngine;


    public class Shooting : MonoBehaviour
    {
        //Gameobject with the position where we will start shooting.
        public GameObject ShotInit;
        //Time between shots
        private float shotDelay;
        //Time when we will be able to shoot again.
        float nextShotTime;
        Animator headAnimator;
        [SerializeField] GameObject headGO;

        bool isShootInput;

        //Animation names
        const string SHOOTING_RIGHT = "ShootingRight";
        const string SHOOTING_LEFT = "ShootingLeft";
        const string SHOOTING_UP = "ShootingUp";
        const string SHOOTING_DOWN = "ShootingDown";
        const string SHOOT_DIRECTION = "LookDirection";
        const string SHOOT_TRIGGER = "Shoot";
        const string IDLE = "IDLE";

        const string LAST_MOVING_DIRECTION = "LastMoving";


        PlayerController playerController;
        private PlayerManager _playerManager;

        //Direction of the next shot if we are using a joystick.
        Vector2 shootInput;

        //Sensibility of the input.
        [SerializeField] float sensibility = 0.3f;
        public AudioSource playerAudioSource;
        Vector2 shootDirection;
      
        public AudioClip tearSound;
        
        CardinalDirection lastShootDirection;
        
        void Start()
        {
            _playerManager = PlayerManager.sharedInstance;
            headAnimator = headGO.GetComponent<Animator>();
            playerController = gameObject.GetComponent<PlayerController>();
            playerAudioSource = PlayerManager.sharedInstance.playerAudioSource;
            lastShootDirection = CardinalDirection.down;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager._instance.pause || _playerManager.dead)
            {
                return;
            }
            //Gets the input in the shotDirection vector2.
            shootInput = playerController.controls.Player.Fire.ReadValue<Vector2>();
            isShootInput = shootInput.magnitude > sensibility;

            if (isShootInput)
            {
                CardinalDirection shootingDirection = GetShootDirection();
                Shoot(shootingDirection);
            }
            else
            //If we are not shooting, changes the animation parameter shooting to false
            //Set the shooting layer weight to 0. 
            {
                // headAnimator.SetBool(IDLE, true);
            }

        }
        CardinalDirection GetShootDirection()
        {
            //Checks which absolute value is bigger between x and y.
            //We use this when using joystick to know if the input is more sided horizontaly or verticaly
            //And change the shoot direction.
            bool shootDirectionHorizontal = Mathf.Abs(shootInput.x) > Mathf.Abs(shootInput.y);
            bool shootDirectionVertical = Mathf.Abs(shootInput.y) > Mathf.Abs(shootInput.x);


            //Gets the direction of the input.
            CardinalDirection shootingDirection = CardinalDirection.nul;

            if ((shootInput.x > sensibility && shootDirectionHorizontal))
                shootingDirection = CardinalDirection.right;
            else if ((shootInput.x < -sensibility && shootDirectionHorizontal))
                shootingDirection = CardinalDirection.left;
            else if (shootInput.y < -sensibility && shootDirectionVertical)
                shootingDirection =  CardinalDirection.down;
            else if (shootInput.y > sensibility && shootDirectionVertical)
                shootingDirection = CardinalDirection.up;
            else
            {
                shootingDirection = lastShootDirection;
            }
            
            return shootingDirection;
        }
        void Shoot(CardinalDirection shootingDirection)
        {
            if (shootingDirection == CardinalDirection.nul) return;
            
            shootDirection = Resources.sharedInstance.cardinalDirections[shootingDirection];
            
            //Reset the animation parameters everytime we change direction
            if(lastShootDirection != shootingDirection)
                ResetAnimationValues();
            
            //Changes the animation to face the shooting direction
            headAnimator.SetInteger(SHOOT_DIRECTION, (int)shootingDirection);
            

            //If there is input, check if the time between shots has elapsed.
            //If so, instantiate a bullet.
            //Reset the nextShotTime
            if (Time.time >= nextShotTime)
            {
                StartCoroutine(InstantiateBullet(shootingDirection));
            }
        }

        IEnumerator InstantiateBullet(CardinalDirection shootingDirection)
        {
            yield return new WaitForEndOfFrame();
            switch (shootingDirection)
            {
                //Those parameters activate the shooting animation.
                //We need to wait until end of frame to activate them because the facing direction animation uses the same parameter.
                case CardinalDirection.down:
                    headAnimator.SetBool(SHOOTING_DOWN, true);
                    break;
                case CardinalDirection.up:
                    headAnimator.SetBool(SHOOTING_UP, true);
                    break;
                case CardinalDirection.right:
                    headAnimator.SetBool(SHOOTING_RIGHT, true);
                    break;
                case CardinalDirection.left:
                    headAnimator.SetBool(SHOOTING_LEFT, true);
                    break;
                case CardinalDirection.nul:
                    yield return null;
                    break;
            }
            
            GameObject tear = Instantiate(PlayerManager.sharedInstance.currentTear, ShotInit.transform.position, Quaternion.identity);
            tear.GetComponent<Tear>().SetBullet(shootDirection, PlayerManager.sharedInstance.shotSpeed);
            lastShootDirection = shootingDirection;
            shotDelay = _playerManager.AttackSpeed;
            nextShotTime = Time.time + shotDelay;
        }

        void ResetAnimationValues()
        {
            headAnimator.SetBool(SHOOTING_LEFT, false);
            headAnimator.SetBool(SHOOTING_UP, false);
            headAnimator.SetBool(SHOOTING_DOWN, false);
            headAnimator.SetBool(SHOOTING_RIGHT, false);
        }
    }


