using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopRoom : MonoBehaviour
{
    //All the item available for the shop
    [SerializeField]private GameObject[] allShopItems;
    //The slot where we will instantiate the items
    [SerializeField]private GameObject[] shopSlots;

    private void Start()
    {
        //Instantiate a random item for each slot in shopSlots
        for (int i = 0; i < shopSlots.Length; i++)
        {
            int randomIndex = Random.Range(0, allShopItems.Length);
            GameObject itemInstance = Instantiate(allShopItems[randomIndex], shopSlots[i].transform.position, Quaternion.identity, shopSlots[i].transform);
            IShoppable shoppableItem = itemInstance.GetComponent<IShoppable>();
            if (shoppableItem != null) shoppableItem.SetShopSlot(shopSlots[i]);
        }
    }
}
