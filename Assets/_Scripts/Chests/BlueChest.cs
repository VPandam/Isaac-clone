using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueChest : Chest
{
    public override void Start()
    {
        base.Start();
        chestItemsList = ItemManager.instance.blueChestLoot;
    }

    public override void OpenChest()
    {
        PlayerManager playerManager = PlayerManager.sharedInstance;
        if (playerManager.currentKeys >= 1)
        {
            base.OpenChest();
            playerManager.UpdateKeys(-1);
        }
    }
}
