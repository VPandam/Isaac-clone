// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerRotatingController : MonoBehaviour
// {
//     Vector2 movement;
//     Vector3 mousePos;
//     Vector2 faceDirection;

//     int lastMoving;

//     public Camera cam;
//     Rigidbody2D rb;

//     public float movementSpeed = 150f;


//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = gameObject.GetComponent<Rigidbody2D>();
//     }

//     // Update is called once per frame
//     void Update()
//     {


//         mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

//         if (Input.GetKey("left") || Input.GetKey("a"))
//         {
//             gameObject.transform.Translate(-movementSpeed * Time.deltaTime, 0, 0);
//             gameObject.GetComponent<Animator>().SetBool("movingLeft", true);
//             gameObject.GetComponent<Animator>().SetBool("moving", true);

//             lastMoving = 4;
//         }

//         if (Input.GetKeyUp("left") || Input.GetKeyUp("a"))
//         {
//             gameObject.GetComponent<Animator>().SetBool("movingLeft", false);
//             gameObject.GetComponent<Animator>().SetBool("moving", false);

//         }

//         if (Input.GetKey("right") || Input.GetKey("d"))
//         {
//             gameObject.transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
//             gameObject.GetComponent<Animator>().SetBool("movingRight", true);
//             gameObject.GetComponent<Animator>().SetBool("moving", true);

//             lastMoving = 3;
//         }

//         if (Input.GetKeyUp("right") || Input.GetKeyUp("d"))
//         {
//             gameObject.GetComponent<Animator>().SetBool("movingRight", false);
//             gameObject.GetComponent<Animator>().SetBool("moving", false);

//         }

//         if (Input.GetKey("up") || Input.GetKey("w"))
//         {
//             gameObject.transform.Translate(0, movementSpeed * Time.deltaTime, 0);
//             gameObject.GetComponent<Animator>().SetBool("movingUp", true);
//             gameObject.GetComponent<Animator>().SetBool("moving", true);

//             lastMoving = 1;
//         }
//         if (Input.GetKeyUp("up") || Input.GetKeyUp("w"))
//         {
//             gameObject.GetComponent<Animator>().SetBool("movingUp", false);
//             gameObject.GetComponent<Animator>().SetBool("moving", false);

//         }

//         if (Input.GetKey("down") || Input.GetKey("s"))
//         {
//             gameObject.transform.Translate(0, -movementSpeed * Time.deltaTime, 0);
//             gameObject.GetComponent<Animator>().SetBool("movingDown", true);
//             gameObject.GetComponent<Animator>().SetBool("moving", true);

//             lastMoving = 2;

//         }

//         if (Input.GetKeyUp("down") || Input.GetKeyUp("s"))
//         {
//             gameObject.GetComponent<Animator>().SetBool("movingDown", false);
//             gameObject.GetComponent<Animator>().SetBool("moving", false);

//         }


//     }

//     private void FixedUpdate()
//     {

//         faceDirection = mousePos - gameObject.transform.position;
//         float angle = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg + 90f;
//         rb.rotation = angle;
//     }
// }
