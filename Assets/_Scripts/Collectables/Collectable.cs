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
    [SerializeField] int ammount;
    public CollectableType _collectableType;
    public int shopPrice;
    public GameObject _shopSlot;
    private PlayerManager _playerManager;
    private AudioSource playerAudioSource;
    [SerializeField]private AudioClip coinClip;

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
                _playerManager.UpdateBombs(ammount);
                break;
            case CollectableType.Coin:
                _playerManager.UpdateCoins(ammount);
                if(playerAudioSource != null && coinClip != null) playerAudioSource.PlayOneShot(coinClip, 0.5f);
                Debug.Log(playerAudioSource != null && coinClip != null);
                break;
            case CollectableType.Key:
                _playerManager.UpdateKeys(ammount);
                break;
            case CollectableType.Heart:
                if (!_playerManager.CheckIfWeCanGetMoreHp(ammount)) return;
                _playerManager.UpdateHp(ammount, HpType.Red);
                break;
            case CollectableType.BlueHeart:
                if (!_playerManager.CheckIfWeCanGetMoreBlueHp(ammount)) return;
                _playerManager.UpdateHp(ammount, HpType.Blue);
                break;
        }
        if (_playerManager.onUIChangeCallback != null)
        {
            _playerManager.onUIChangeCallback.Invoke();
        }
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
        if (_collectableType == CollectableType.Heart && !_playerManager.CheckIfWeCanGetMoreHp(ammount)) return;
        
        if (_collectableType == CollectableType.BlueHeart && !_playerManager.CheckIfWeCanGetMoreBlueHp(ammount)) return;
        
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
