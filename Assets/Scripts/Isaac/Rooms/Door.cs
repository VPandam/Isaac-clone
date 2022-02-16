using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorPos
{
    Up, Down, Right, Left
}

public class Door : MonoBehaviour
{
    public bool open;
    BoxCollider2D doorCollider;
    Animator animator;

    public DoorPos doorPos;

    public
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        doorCollider = gameObject.GetComponent<BoxCollider2D>();
        open = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            OpenDoor();
        }
        else
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

    public void SetOpen(bool value)
    {
        open = value;
    }
}
