using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooting : MonoBehaviour
{
    //Prefab of the tear to shoot.
    public GameObject currentTear;
    //Gameobject with the position where we will start shooting.
    public GameObject ShotInit;
    //Time between shots
    private float shotDelay;
    //Time when we will be able to shoot again.
    float nextShotTime;
    Animator animator;

    //Axis names
    const string FIRE_HORIZONTAL = "FireHorizontal";
    const string FIRE_VERTICAL = "FireVertical";
    const string FIRE_HORIZONTAL_KEYS = "FireHorizontalKeys";
    const string FIRE_VERTICAL_KEYS = "FireVerticalKeys";

    //Animation names
    const int SHOOTING_LAYER_INDEX = 1;
    const string SHOOTING = "Shooting";
    const string SHOOT_HORIZONTAL = "ShootHorizontal";
    const string SHOOT_VERTICAL = "ShootVertical";
    const string LAST_MOVING_DIRECTION = "LastMoving";


    PlayerController playerController;

    //Direction of the next shot if we are using a joystick.
    Vector2 shotDirection;
    //Direction of the next shot if we are using the keyboard.
    Vector2 shotDirectionKeys;

    //Sensibility of the input.
    float sensibility = 0.3f;
    //Angle in degrees of the next shot.
    //Changes depending of the input.
    int degreesShot;

    //True if there is any input bigger than the sensibility.
    bool shooting;
    //Gets if we are using joystick or keyboard.
    bool usingJoystick, usingKeyboard;
    //Checks if there's input.
    bool joystickNotMoving, keysNotMoving;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        playerController = gameObject.GetComponent<PlayerController>();
        shotDelay = PlayerStats.instance.FireRate;
        if (shotDelay == 0)
        {
            shotDelay = 0.5f;
        }
        currentTear = PlayerStats.instance.CurrentTear;

    }

    // Update is called once per frame
    void Update()
    {
        //Gets the input in the shotDirection vector2.
        shotDirection.x = Input.GetAxis(FIRE_HORIZONTAL);
        shotDirection.y = Input.GetAxis(FIRE_VERTICAL);
        shotDirectionKeys.x = Input.GetAxis(FIRE_HORIZONTAL_KEYS);
        shotDirectionKeys.y = Input.GetAxis(FIRE_VERTICAL_KEYS);

        //Checks if we are using keyboard or joystick.
        usingJoystick = playerController.UsingJoystick;
        usingKeyboard = playerController.UsingKeyboard;

        //Check if there's input bigger than sensibility.
        joystickNotMoving = shotDirection.magnitude < sensibility;
        keysNotMoving = shotDirectionKeys.magnitude < sensibility;

        shooting = false;
        //Update the horizontal and vertical shoot parameters in the animator.
        //And set the shooting bool to true.
        //For joystick.
        if (playerController.UsingJoystick && !joystickNotMoving)
        {
            Debug.Log(shotDirection.x + " Vertical " + shotDirection.y);

            animator.SetFloat(SHOOT_HORIZONTAL, shotDirection.x);
            animator.SetFloat(SHOOT_VERTICAL, shotDirection.y);
            shooting = true;

        }
        //Update the horizontal and vertical shoot parameters in the animator.
        //And set the shooting bool to true.
        //For keyboard.
        else if (playerController.UsingKeyboard && !keysNotMoving)
        {
            Debug.Log(shotDirectionKeys.x + " Vertical " + shotDirectionKeys.y);

            animator.SetFloat(SHOOT_HORIZONTAL, shotDirectionKeys.x);
            animator.SetFloat(SHOOT_VERTICAL, shotDirectionKeys.y);
            shooting = true;

        }

        //Checks which absolute value is bigger netween x and y.
        //We use this when using joystick to know if the input is more sided horizontaly or verticaly
        //And change the shoot direction.
        bool shootDirectionHorizontal = usingJoystick && Mathf.Abs(shotDirection.x) > Mathf.Abs(shotDirection.y);
        bool shootDirectionVertical = usingJoystick && Mathf.Abs(shotDirection.y) > Mathf.Abs(shotDirection.x);


        //Gets the direction of the input.
        bool shootingRightInput = ((shotDirection.x > sensibility && usingJoystick && shootDirectionHorizontal)
        || (shotDirectionKeys.x > sensibility && usingKeyboard));

        bool shootingLeftInput = ((shotDirection.x < -sensibility && usingJoystick && shootDirectionHorizontal)
        || (shotDirectionKeys.x < -sensibility && usingKeyboard));

        bool shootingUpInput = ((shotDirection.y < -sensibility && usingJoystick && shootDirectionVertical)
        || (shotDirectionKeys.y < -sensibility && usingKeyboard));

        bool shootingDownInput = ((shotDirection.y > sensibility && usingJoystick && shootDirectionVertical)
        || (shotDirectionKeys.y > sensibility && usingKeyboard));



        if (shooting)
        {
            //If we are shooting change the animation to the shooting animation
            animator.SetLayerWeight(SHOOTING_LAYER_INDEX, 1);
            animator.SetBool(SHOOTING, true);

            //Change the bullet angle depending on the direction of the input.
            //Sets the lastMovingDirection parameter of the animator, this determines the
            //IDLE animation to run if we are not shooting or moving.
            if (shootingDownInput)
            {
                degreesShot = 90;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.up);
            }
            if (shootingUpInput)
            {
                degreesShot = -90;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.down);
            }

            if (shootingLeftInput)
            {
                degreesShot = -180;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.left);
            }
            if (shootingRightInput)
            {
                degreesShot = 0;
                animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.right);
            }
        }

        //If we are there is input, check if the time between shots has elapsed.
        //If so, instantiate a bullet.
        //Reset the nextShotTime
        if (Time.time >= nextShotTime && shooting)
        {

            Shoot();

        }

        //If we are not shooting, changes the animation parameter shooting to false
        //Set the shooting layer weight to 0. 
        if ((joystickNotMoving && playerController.UsingJoystick) ||
        (keysNotMoving && playerController.UsingKeyboard))
        {
            animator.SetBool(SHOOTING, false);
            animator.SetLayerWeight(SHOOTING_LAYER_INDEX, 0);
        }
    }
    void Shoot()
    {
        Instantiate(currentTear, ShotInit.transform.position, Quaternion.AngleAxis(degreesShot, Vector3.forward));
        nextShotTime = Time.time + shotDelay;
    }

}
