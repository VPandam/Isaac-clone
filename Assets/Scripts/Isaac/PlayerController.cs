using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject ShotInit;
    float movementSpeed;



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

        

        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    if (lastMoving == 2)
        //    {
        //        Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(-90, Vector3.forward));
        //    }
            
        //    if (lastMoving == 4)
        //    {
        //        Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(180, Vector3.forward));
        //    }
        //    if (lastMoving == 3)
        //    {
        //        Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(0, Vector3.forward));
        //    }
        //    if (lastMoving == 1)
        //    {
        //        Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(90, Vector3.forward));
        //    }

        //}


        
    }
    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.tag.Equals("ExitRight"))
        {
            RoomsController.instance.NewRoom("right");
        }
    }
 
}
