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
            if (item) Instantiate(item, itemSpawn.transform.position, Quaternion.identity);
        }
 



    }


