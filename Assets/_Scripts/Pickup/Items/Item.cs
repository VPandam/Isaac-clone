using System;
using UnityEngine;
using UnityEngine.Serialization;


// public interface Iitem
    // {
    //     public void DoEffect();
    // }
    public class Item : MonoBehaviour, IShoppable
    {
        public string effectDescription;
        public string _name;
        public Sprite _sprite;
        private SpriteRenderer _spriteRenderer;
        public readonly Vector2 itemSize = new Vector2(1, 1);
        [HideInInspector] public int id;
        public int shopPrice;
    //Some items can appear in shops, this will get an i nstance of the shop slot if its the case.
    public GameObject _shopSlot;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _sprite;
            _spriteRenderer.size = itemSize;
        }

        public virtual void DoEffect()
        {
            ItemManager.instance.RegisterItemAsUsed(this);
            Debug.Log(effectDescription);
        }

        public void BuyItem()
        {
            PlayerManager playerManager = PlayerManager.sharedInstance;
            if (playerManager.currentCoins >= shopPrice)
            {
                playerManager.UpdateCoins(-shopPrice);
                DoEffect();
                Destroy(_shopSlot);        
            }
        }

        public void SetShopSlot(GameObject shopSlot)
        {
            _shopSlot = shopSlot;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (_shopSlot != null) BuyItem();
                else
                {  
                Debug.Log(_name + " picked up");
                DoEffect();
                    Destroy(this.gameObject);
                }
            }
        }
    }



