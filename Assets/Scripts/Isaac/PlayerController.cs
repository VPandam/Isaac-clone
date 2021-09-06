using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject ShotInit;
    float movementSpeed;

    Vector3 moveTo;



    private void Start()
    {
        movementSpeed = PlayerStats.instance.MoveSpeed;
    }
    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetKey("a"))
        {
            gameObject.transform.Translate(-movementSpeed * Time.deltaTime, 0, 0);
            gameObject.GetComponent<Animator>().SetBool("movingLeft", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);
            
        }

        if (Input.GetKeyUp("a"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingLeft", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
            
        }

        if (Input.GetKey("d"))
        {
            gameObject.transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
            gameObject.GetComponent<Animator>().SetBool("movingRight", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);

        }

        if (Input.GetKeyUp("d"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingRight", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
            
        }

        if (Input.GetKey("w"))
        {
            gameObject.transform.Translate(0, movementSpeed * Time.deltaTime, 0);
            gameObject.GetComponent<Animator>().SetBool("movingUp", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);
            

        }
        if (Input.GetKeyUp("w"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingUp", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
            
        }

        if (Input.GetKey("s"))
        {
            gameObject.transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
            gameObject.GetComponent<Animator>().SetBool("movingDown", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);
            

        }

        if (Input.GetKeyUp("s"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingDown", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
            
        }

        
    }
    private void OnTriggerExit2D (Collider2D collision)
    {
        
        //if (collision.tag.Equals("ExitRight"))
        //{
        //    RoomsController.instance.NewRoom("right");
        //}

        //if (collision.tag.Equals("ExitLeft"))
        //{
        //    RoomsController.instance.NewRoom("left");
        //}

        //if (collision.tag.Equals("ExitUp"))
        //{
        //    RoomsController.instance.NewRoom("up");
        //}

        //if (collision.tag.Equals("ExitDown"))
        //{
        //    RoomsController.instance.NewRoom("down");
        //}

    }

    public void MoveTo(Vector3 to)
    {

    }

}
