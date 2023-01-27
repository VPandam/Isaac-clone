using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    Bomb, Key, Coin, Heart, BlueHeart
}
public class Collectable : MonoBehaviour, IShoppable
{
    [SerializeField] int amount;
    public CollectableType _collectableType;
    public int shopPrice;
    public GameObject _shopSlot;
    private PlayerManager _playerManager;
    private AudioSource playerAudioSource;
    [SerializeField]private AudioClip collectableClip;

    private void Start()
    {
        _playerManager = PlayerManager.sharedInstance;
        playerAudioSource = _playerManager.GetComponent<AudioSource>();
    }

    void Collect()
    {
        switch (_collectableType)
        {
            case CollectableType.Bomb:
                _playerManager.UpdateBombs(amount);
                break;
            case CollectableType.Coin:
                _playerManager.UpdateCoins(amount);
                
                break;
            case CollectableType.Key:
                _playerManager.UpdateKeys(amount);
                break;
            case CollectableType.Heart:
                //CHeck if we can get more hp
                if(amount > 0)
                    if (!_playerManager.CheckIfWeCanGetMoreHp()) return;
                _playerManager.UpdateHp(amount, HpType.Red);
                break;
            case CollectableType.BlueHeart:
                //Check if we can get more blue hp
                if(amount > 0)
                    if (!_playerManager.CheckIfWeCanGetMoreBlueHp()) return;
                _playerManager.UpdateHp(amount, HpType.Blue);
                break;
        }
        if (_playerManager.onUIChangeCallback != null)
        {
            _playerManager.onUIChangeCallback.Invoke();
        }
        if(playerAudioSource != null && collectableClip != null) playerAudioSource.PlayOneShot(collectableClip, 0.2f);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(_shopSlot != null) BuyItem();
            else
                Collect();
        }
    }

    public void BuyItem()
    {
        if (_collectableType == CollectableType.Heart && !_playerManager.CheckIfWeCanGetMoreHp()) return;
        
        if (_collectableType == CollectableType.BlueHeart && !_playerManager.CheckIfWeCanGetMoreBlueHp()) return;
        
        if (_playerManager.currentCoins >= shopPrice)
        {
            _playerManager.UpdateCoins(-shopPrice);
            Collect();
            Destroy(_shopSlot);        
        }
        
    }

    public void SetShopSlot(GameObject shopSlot)
    {
        _shopSlot = shopSlot;
    }
}
