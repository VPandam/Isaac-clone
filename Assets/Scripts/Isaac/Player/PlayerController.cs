using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingDirection
{
    up, down, right, left
}
public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;
    float movementSpeed;

    CameraController cam;

    Animator animator;

    float horizontal, vertical;
    float horizontalKeys, verticalKeys;


    const string MOVING = "Moving";
    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";
    const string LAST_MOVING_DIRECTION = "LastMoving";

    bool usingJoystick;
    public bool UsingJoystick { get { return usingJoystick; } set { usingJoystick = value; } }
    bool usingKeyboard;
    public bool UsingKeyboard { get { return usingKeyboard; } set { usingKeyboard = value; } }

    //The amount of force you need to apply to the joyastick to be capted
    public float sensibility = 0.3f;

    Rigidbody2D rb;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {

        animator = gameObject.GetComponent<Animator>();
        movementSpeed = PlayerStats.instance.MoveSpeed;
        cam = Camera.main.GetComponent<CameraController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        horizontalKeys = Input.GetAxisRaw("HorizontalKeys");
        vertical = Input.GetAxisRaw("Vertical");
        verticalKeys = Input.GetAxisRaw("VerticalKeys");

        bool joystickNotMoving = horizontal < sensibility && horizontal > -sensibility && vertical < sensibility && vertical > -sensibility;
        bool keysNotMoving = horizontalKeys < sensibility && horizontalKeys > -sensibility && verticalKeys < sensibility && verticalKeys > -sensibility;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SwapToKeyboard();
            Debug.Log("Using keyboard");
        }
        if (Input.GetButtonDown("Submit"))
        {
            SwapToJoystick();
            Debug.Log("Using Joystick");
        }


        if (usingJoystick && !joystickNotMoving)
        {
            animator.SetFloat(HORIZONTAL, horizontal);
            animator.SetFloat(VERTICAL, vertical);
            animator.SetBool(MOVING, true);
            Vector3 movement = new Vector3(horizontal, vertical, 0).normalized;
            transform.Translate(movement * Time.deltaTime * movementSpeed);
        }

        if (usingKeyboard && !keysNotMoving)
        {
            animator.SetFloat(HORIZONTAL, horizontalKeys);
            animator.SetFloat(VERTICAL, verticalKeys);
            animator.SetBool(MOVING, true);
            transform.Translate(movementSpeed * Time.deltaTime * horizontalKeys, movementSpeed * Time.deltaTime * verticalKeys, 0);
        }

        bool movingRightInput = ((horizontal > sensibility && usingJoystick) || (horizontalKeys > sensibility && usingKeyboard));
        bool movingLeftInput = ((horizontal < -sensibility && usingJoystick) || (horizontalKeys < -sensibility && usingKeyboard));
        bool movingUpInput = ((vertical < -sensibility && usingJoystick) || (verticalKeys < -sensibility && usingKeyboard));
        bool movingDownInput = ((vertical > sensibility && usingJoystick) || (verticalKeys > sensibility && usingKeyboard));

        /// There are 4 different IDLE aniamtions.
        /// Last moving direction is a parameter used in the player animator to activate each animation.
        if (movingRightInput)
        {
            animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.right);
        }
        if (movingLeftInput)
        {
            animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.left);
        }
        if (movingUpInput)
        {
            animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.down);
        }
        if (movingDownInput)
        {
            animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.up);
        }


        if ((joystickNotMoving && usingJoystick) ||
        (keysNotMoving && usingKeyboard))
        {
            animator.SetBool(MOVING, false);
        }


    }
    public void SwapToKeyboard()
    {
        usingKeyboard = true;
        usingJoystick = false;
    }
    public void SwapToJoystick()
    {
        usingKeyboard = false;
        usingJoystick = true;
    }

    /// <summary>
    /// When crossing a door, move the camera to the new room.
    /// Change the player position to the new room player spawn. 
    /// Destroy all bullets
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DoorExitZone"))
        {

            ExitZone exitZone = collision.GetComponent<ExitZone>();
            Room roomToSpawn = exitZone.roomToSpawn;
            if (roomToSpawn)
            {
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
                foreach (var bullet in bullets)
                {
                    Destroy(bullet);
                }

                cam.MoveCameraTo(roomToSpawn.transform.position);
                if (!roomToSpawn.playerEntered)
                {
                    roomToSpawn.playerEntered = true;
                    roomToSpawn.Invoke("StartRoom", 1f);
                }
                this.gameObject.transform.position = exitZone.playerSpawnPosition;
            }
        }
    }
}
