using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState{
    open,close
}
public class Door : MonoBehaviour
{
    public DoorState currentState;
    BoxCollider2D doorCollider;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        doorCollider = gameObject.GetComponent<BoxCollider2D>();
        currentState = DoorState.open;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == DoorState.open)
        {
            OpenDoor();
        }
        if (currentState == DoorState.close)
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        doorCollider.enabled = false;
        animator.SetBool("Open", true);
    }

    void CloseDoor()
    {
        doorCollider.enabled = true;
        animator.SetBool("Open", false);
    }

    public void SetCurrentState (DoorState value)
    {
        this.currentState = value;
    }
}
