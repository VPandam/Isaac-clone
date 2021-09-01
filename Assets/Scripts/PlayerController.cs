using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float movementSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            gameObject.transform.Translate(-movementSpeed * Time.deltaTime, 0, 0);
            gameObject.GetComponent<Animator>().SetBool("movingLeft", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);
        }
        if (Input.GetKeyUp("left") || Input.GetKeyUp("a"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingLeft", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            gameObject.transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
            gameObject.GetComponent<Animator>().SetBool("movingRight", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);
        }
        if (Input.GetKeyUp("right") || Input.GetKeyUp("d"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingRight", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
        }
        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            gameObject.transform.Translate(0, movementSpeed * Time.deltaTime, 0);
            gameObject.GetComponent<Animator>().SetBool("movingUp", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);
        }
        if (Input.GetKeyUp("up") || Input.GetKeyUp("w"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingUp", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
        }

        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            gameObject.transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
            gameObject.GetComponent<Animator>().SetBool("movingDown", true);
            gameObject.GetComponent<Animator>().SetBool("moving", true);

        }
        if (Input.GetKeyUp("down") || Input.GetKeyUp("s"))
        {
            gameObject.GetComponent<Animator>().SetBool("movingDown", false);
            gameObject.GetComponent<Animator>().SetBool("moving", false);
        }



    }
}
