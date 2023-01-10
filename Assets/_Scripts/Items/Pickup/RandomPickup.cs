using UnityEngine;

    public class RandomPickup : MonoBehaviour
    {
        Item item;
        SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject itemSpawn;
        private void Start()
        {
            item = ItemManager.instance.GetRandomItem();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (item)
            {
                Item itemInstance = Instantiate(item, itemSpawn.transform.position, Quaternion.identity);
                itemInstance.transform.SetParent(itemSpawn.transform);
            }
        }
 



    }


