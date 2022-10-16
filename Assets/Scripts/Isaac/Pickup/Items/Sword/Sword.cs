using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    PlayerManager playerStats;
    override public void DoEffect()
    {
        base.DoEffect();
        playerStats = PlayerManager.instance;
        playerStats.attackDamage += 2;
        playerStats.currentTear.GetComponent<SpriteRenderer>().color = Color.blue;
        playerStats.currentTear.transform.localScale = new Vector3(2, 2, 2);
    }
}
