using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    Bomb, Key, Coin, Heart, BlueHeart
}
public class Collectable : MonoBehaviour
{
    [SerializeField] int ammount;
    public CollectableType _collectableType;


    void Collect(PlayerManager playerManager)
    {
        switch (_collectableType)
        {
            case CollectableType.Bomb:
               if (playerManager.currentBombs ! >= playerManager.maxCollectables)
                    playerManager.currentBombs += ammount;
               break;
            case CollectableType.Coin:
                if (playerManager.currentCoins ! >= playerManager.maxCollectables)
                    playerManager.currentCoins += ammount;
                break;
            case CollectableType.Key:
                if (playerManager.currentBombs ! >= playerManager.maxCollectables)
                 playerManager.currentKeys += ammount;
                break;
            case CollectableType.Heart:
                playerManager.UpdateHp(ammount, HpType.Red);
                break;
            case CollectableType.BlueHeart:
                playerManager.UpdateHp(ammount, HpType.Blue);
                break;
        }
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            Collect(playerManager);
        }
    }
}
