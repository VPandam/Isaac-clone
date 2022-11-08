using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovingDirection
{
    up, down, right, left
}
public class PlayerMovement : MonoBehaviour
{
    //Components
    static public PlayerMovement _instance;
    CameraController _cam;

    public Animator _animator;
    Rigidbody2D _rb;



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



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        controls = new InputActions();
        controls.Enable();

        controls.Player.Move.canceled += ctxt =>
        {
            movementInput = Vector2.zero;
        };
        controls.Player.UpdateHp.performed += ctxt =>
        {
            PlayerManager.instance.TakeDamage(1);
        };


    }
    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _cam = Camera.main.GetComponent<CameraController>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (GameManager._instance.pause)
        {
            _animator.SetBool(MOVING, false);

            return;
        }

        movementInput = controls.Player.Move.ReadValue<Vector2>();

        bool movingRightInput = (movementInput.x > sensibility);
        bool movingLeftInput = (movementInput.x < -sensibility);
        bool movingUpInput = (movementInput.y < -sensibility);
        bool movingDownInput = (movementInput.y > sensibility);

        /// There are 4 different IDLE aniamtions.
        /// Last moving direction is a parameter used in the player animator to activate each animation.
        if (movingRightInput)
        {
            _animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.right);
        }
        if (movingLeftInput)
        {
            _animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.left);
        }
        if (movingUpInput)
        {
            _animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.down);
        }
        if (movingDownInput)
        {
            _animator.SetInteger(LAST_MOVING_DIRECTION, (int)MovingDirection.up);
        }


        if (!moving)
        {
            _animator.SetBool(MOVING, false);
            _rb.velocity = Vector2.zero;
        }


    }
    private void FixedUpdate()
    {
        if (GameManager._instance.pause)
        {
            _animator.SetBool(MOVING, false);
            return;
        }

        moving = movementInput.normalized.magnitude > sensibility;

        if (movementInput.normalized.magnitude > sensibility)
            Move();
    }
    void Move()
    {
        float movementSpeed = PlayerManager.instance.moveSpeed;
        _animator.SetFloat(HORIZONTAL, movementInput.x);
        _animator.SetFloat(VERTICAL, movementInput.y);
        _animator.SetBool(MOVING, true);
        _rb.MovePosition(_rb.position + movementInput * movementSpeed * Time.fixedDeltaTime);
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
            ChangeRoom(collision);
        }
    }
    void ChangeRoom(Collider2D collision)
    {
        ExitZone exitZone = collision.GetComponent<ExitZone>();
        Room roomToSpawn = exitZone.roomToSpawn;

        if (roomToSpawn)
        {
            //Fade in black screen
            GameManager._instance.StartCoroutine("FadeInFadeOut");

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
                roomToSpawn.Invoke("StartRoom", 1f);
            }

            //Move the player to the new room spawn position
            gameObject.transform.position = exitZone.playerSpawnPosition;
        }
    }



    private void OnDisable()
    {
        controls.Disable();
    }

}
