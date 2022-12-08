using UnityEngine;

    public class RandomPickup : MonoBehaviour
    {
        Item item;
        SpriteRenderer spriteRenderer;
        private void Start()
        {
            item = ItemManager.instance.GetRandomItem();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (item._sprite)
                spriteRenderer.sprite = item._sprite;
            spriteRenderer.size = new Vector2(0.5f,0.5f);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(item._name + " picked up");
                item.DoEffect();
                Destroy(this.gameObject);
            }
        }



    }


