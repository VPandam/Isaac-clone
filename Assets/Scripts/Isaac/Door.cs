using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState{
    open,close
}
public class Door : MonoBehaviour
{
    public DoorState currentState;
    GameObject collider;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        collider = GameObject.Find("DoorCollider");
        currentState = DoorState.open;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState.Equals(DoorState.open))
        {
            OpenDoor();
        }
        if (currentState.Equals(DoorState.close))
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        collider.GetComponent<Collider2D>().enabled = false;
        animator.SetBool("Open", true);
    }

    void CloseDoor()
    {
        collider.GetComponent<Collider2D>().enabled = true;
        animator.SetBool("Open", false);
    }

    public void SetCurrentState (DoorState value)
    {
        this.currentState = value;
    }
}
