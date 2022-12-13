using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public List<GameObject> roomLoot = new List<GameObject>();
    //All the item available for the shop
    public GameObject[] allShopItems;
    public List<GameObject> chestLoot = new List<GameObject>();
    public List<GameObject> blueChestLoot = new List<GameObject>();
    public List<Item> allItems = new List<Item>();
    public List<Item> itemsUsed = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        for (int i = 0; i < itemsUsed.Count; i++)
        {
            allItems[i].id = i;
        }
    }

    public Item GetRandomItem()
    {
        int random = Random.Range(0, allItems.Count);
        if (allItems.Count > 0)
        {
            Item returnItem = allItems[random];
            itemsUsed.Add(returnItem);
            allItems.Remove(returnItem);
            return returnItem;
        }
        return null;

    }
}
    

