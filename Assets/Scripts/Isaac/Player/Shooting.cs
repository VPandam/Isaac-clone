using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooting : MonoBehaviour
{
    enum ShootingDirection
    {
        up, down, left, right, nul
    }
    //Prefab of the tear to shoot.
    public GameObject currentTear;
    //Gameobject with the position where we will start shooting.
    public GameObject ShotInit;
    //Time between shots
    private float shotDelay;
    //Time when we will be able to shoot again.
    float nextShotTime;
    Animator animator;

    bool isShootInput;

    //Animation names
    const int SHOOTING_LAYER_INDEX = 1;
    const string SHOOTING = "Shooting";
    const string SHOOT_DIRECTION = "ShootDirection";

    const string LAST_MOVING_DIRECTION = "LastMoving";


    PlayerMovement playerController;

    //Direction of the next shot if we are using a joystick.
    Vector2 shootInput;

    //Sensibility of the input.
    [SerializeField] float sensibility = 0.3f;


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerMovement>();
        shotDelay = PlayerManager.instance.fireRate;
        if (shotDelay == 0)
        {
            shotDelay = 0.5f;
        }
        currentTear = PlayerManager.instance.currentTear;

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager._instance.pause)
        {
            animator.SetBool(SHOOTING, false);
            animator.SetLayerWeight(SHOOTING_LAYER_INDEX, 0);
            return;
        }
        //Gets the input in the shotDirection vector2.
        shootInput = playerController.controls.Player.Fire.ReadValue<Vector2>();

        isShootInput = shootInput.normalized.magnitude > sensibility;



        if (isShootInput)
        {
            ShootingDirection shootingDirection = GetShootDirection();
            Shoot(shootingDirection);
        }
        else
        //If we are not shooting, changes the animation parameter shooting to false
        //Set the shooting layer weight to 0. 
        {
            animator.SetBool(SHOOTING, false);
            animator.SetLayerWeight(SHOOTING_LAYER_INDEX, 0);
        }

    }
    ShootingDirection GetShootDirection()
    {
        //Checks which absolute value is bigger between x and y.
        //We use this when using joystick to know if the input is more sided horizontaly or verticaly
        //And change the shoot direction.
        bool shootDirectionHorizontal = Mathf.Abs(shootInput.x) > Mathf.Abs(shootInput.y);
        bool shootDirectionVertical = Mathf.Abs(shootInput.y) > Mathf.Abs(shootInput.x);


        //Gets the direction of the input.

        ShootingDirection shootingDirection = ShootingDirection.nul;

        if ((shootInput.x > sensibility && shootDirectionHorizontal))
            shootingDirection = ShootingDirection.right;

        if ((shootInput.x < -sensibility && shootDirectionHorizontal))
            shootingDirection = ShootingDirection.left;

        if (shootInput.y < -sensibility && shootDirectionVertical)
            shootingDirection = ShootingDirection.down;

        if (shootInput.y > sensibility && shootDirectionVertical)
            shootingDirection = ShootingDirection.up;

        return shootingDirection;
    }
    void Shoot(ShootingDirection shootingDirection)
    {
        //We want the shoot animation to prioritize shooting animation over movement.
        //If we are shooting change the layer to the shooting animation
        animator.SetLayerWeight(SHOOTING_LAYER_INDEX, 1);
        animator.SetBool(SHOOTING, true);
        animator.SetInteger(SHOOT_DIRECTION, (int)shootingDirection);

        int degreesShot = 0;
        //Change the bullet angle depending on the direction of the input.
        //Sets the lastMovingDirection parameter of the animator, this determines the
        //IDLE animation to run if we are not shooting or moving.
        switch (shootingDirection)
        {

            default:
                break;
            case ShootingDirection.up:
                degreesShot = 90;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.up);
                break;

            case ShootingDirection.down:
                degreesShot = -90;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.down);
                break;

            case ShootingDirection.left:
                degreesShot = -180;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.left);
                break;

            case ShootingDirection.right:
                degreesShot = 0;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.right);
                break;
            case ShootingDirection.nul:
                return;

        }


        //If there is input, check if the time between shots has elapsed.
        //If so, instantiate a bullet.
        //Reset the nextShotTime
        if (Time.time >= nextShotTime)
        {
            Instantiate(currentTear, ShotInit.transform.position, Quaternion.AngleAxis(degreesShot, Vector3.forward));
            nextShotTime = Time.time + shotDelay;
        }





    }
}
