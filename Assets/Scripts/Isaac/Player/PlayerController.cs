using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovingDirection
{
    up, down, right, left
}
public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;
    [SerializeField] float movementSpeed = 3;

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

    // Controller controls;
    public InputActions controls;
    Vector2 controlsMovement;
    Rigidbody2D rb;
    bool moving;
    Vector2 movementInput;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        controls = new InputActions();
        controls.Enable();

        controls.Player.Move.canceled += ctxt =>
        {
            movementInput = Vector2.zero;
        };


    }
    private void Start()
    {

        animator = gameObject.GetComponent<Animator>();
        movementSpeed = PlayerStats.instance.MoveSpeed;
        cam = Camera.main.GetComponent<CameraController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        movementInput = controls.Player.Move.ReadValue<Vector2>();



        bool movingRightInput = (movementInput.x > sensibility);
        bool movingLeftInput = (movementInput.x < -sensibility);
        bool movingUpInput = (movementInput.y < -sensibility);
        bool movingDownInput = (movementInput.y > sensibility);

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


        if (!moving)
        {
            animator.SetBool(MOVING, false);
        }


    }
    private void FixedUpdate()
    {
        moving = movementInput.normalized.magnitude > sensibility;

        if (movementInput.normalized.magnitude > sensibility)
            Move();
    }
    void Move()
    {
        animator.SetFloat(HORIZONTAL, movementInput.x);
        animator.SetFloat(VERTICAL, movementInput.y);
        animator.SetBool(MOVING, true);
        rb.MovePosition(rb.position + movementInput * movementSpeed * Time.deltaTime);
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
    private void OnDisable()
    {
        controls.Disable();
    }
}
