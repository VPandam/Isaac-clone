using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Pickup/Item/Sword")]
public class Sword : Item
{
    PlayerStats playerStats;
    public float attackDamageModifier = 2;

    private void Start()
    {
    }
    public override void DoEffect()
    {
        base.DoEffect();

        playerStats = PlayerStats.instance;
        playerStats.AttackDamage += 2;
        playerStats.CurrentTear.GetComponent<SpriteRenderer>().color = Color.blue;
        playerStats.CurrentTear.transform.localScale = new Vector3(2, 2, 2);

    }
}
