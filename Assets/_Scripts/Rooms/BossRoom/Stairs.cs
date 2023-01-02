using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Stairs");
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager._instance.LoadNewLevel();
        }    
    }
}
