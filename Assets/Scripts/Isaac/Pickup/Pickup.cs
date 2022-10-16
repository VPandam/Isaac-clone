using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    Item item;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        item = ItemManager.instance.GetRandomItem();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (item._sprite)
            spriteRenderer.sprite = item._sprite;
        spriteRenderer.size = item.itemSize;
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
