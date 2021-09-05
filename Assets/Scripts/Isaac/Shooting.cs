using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum shootDirection
{
    up,
    down, 
    right,
    left 
}
public class Shooting : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject ShotInit;

    public float shotDelay;
    float nextShotTime;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (shotDelay == 0)
        {
            shotDelay = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right"))
        {
            if( Time.time >= nextShotTime)
            {
                Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(0, Vector3.forward));
                nextShotTime = Time.time + shotDelay;
            }

            setBoolFalse(shootDirection.right);
            gameObject.GetComponent<Animator>().SetBool("shooting", true);
            animator.SetBool("movingRight", true);
        }
        if (Input.GetKeyUp("right"))
        {
            animator.SetBool("movingRight", false);
            gameObject.GetComponent<Animator>().SetBool("shooting", false);

        }

        if (Input.GetKey("left"))
        {
            if (Time.time >= nextShotTime)
            {
                Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(180, Vector3.forward));
                nextShotTime = Time.time + shotDelay;
            
            }
            
            setBoolFalse(shootDirection.left);
            gameObject.GetComponent<Animator>().SetBool("shooting", true);
            animator.SetBool("movingLeft", true);

        }
        if (Input.GetKeyUp("left"))
        {
            animator.SetBool("movingLeft", false);
            gameObject.GetComponent<Animator>().SetBool("shooting", false);

        }

        if (Input.GetKey("up"))
        {
            if (Time.time >= nextShotTime)
            {
                Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(90, Vector3.forward));
                nextShotTime = Time.time + shotDelay;

            }
            
            setBoolFalse(shootDirection.up);
            gameObject.GetComponent<Animator>().SetBool("shooting", true);
            animator.SetBool("movingUp", true);

        }
        if (Input.GetKeyUp("up"))
        {
            animator.SetBool("movingUp", false);
            gameObject.GetComponent<Animator>().SetBool("shooting", false);

        }

        if (Input.GetKey("down"))
        {
            if (Time.time >= nextShotTime)
            {
                Instantiate(Arrow, ShotInit.transform.position, Quaternion.AngleAxis(-90, Vector3.forward)); ;
                nextShotTime = Time.time + shotDelay;

            }
            
            setBoolFalse(shootDirection.down);
            gameObject.GetComponent<Animator>().SetBool("shooting", true);
            animator.SetBool("movingDown", true);

        }
        if (Input.GetKeyUp("down"))
        {
            animator.SetBool("movingDown", false);
            gameObject.GetComponent<Animator>().SetBool("shooting", false);

        }
    }

    void setBoolFalse(shootDirection shootDirection)
    {
        switch (shootDirection)
        {
            case shootDirection.up:
                animator.SetBool("movingRight", false);
                animator.SetBool("movingLeft", false);
                animator.SetBool("movingDown", false);
                break;
            case shootDirection.down:
                animator.SetBool("movingRight", false);
                animator.SetBool("movingLeft", false);
                animator.SetBool("movingUp", false);
                break;
            case shootDirection.right:
                animator.SetBool("movingUp", false);
                animator.SetBool("movingLeft", false);
                animator.SetBool("movingDown", false);
                break;
            case shootDirection.left:
                animator.SetBool("movingRight", false);
                animator.SetBool("movingUp", false);
                animator.SetBool("movingDown", false);
                break;

        }
    }
}
