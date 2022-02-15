using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    public Item pickupSO;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(pickupSO._name + " picked up");
            pickupSO.DoEffect();
            Destroy(this.gameObject);
        }
    }



}
