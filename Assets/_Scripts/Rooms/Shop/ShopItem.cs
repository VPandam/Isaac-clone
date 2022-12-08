using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private Collectable _collectable;
    private Item _item;
    private int price;
    [SerializeField] private TextMeshProUGUI priceText;
    private Collider2D _collider;
    

    private void Start()
    {
        _item = GetComponentInChildren<Item>();
        _collectable = GetComponentInChildren<Collectable>();
        if (_collectable != null) price = _collectable.shopPrice;
        if (_item != null) price = _item.shopPrice;
        priceText.text = price.ToString();
        _collider = GetComponentInChildren<Collider2D>();
    }

    private void OnTriggerEnter2d(Collider other)
    {
        if(other.CompareTag("Player"))
            Destroy(gameObject, 0.1f);
    }
}
