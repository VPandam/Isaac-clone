using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Item
{
    [SerializeField] private float attackSpeedAmount;
    public override void DoEffect()
    {
        base.DoEffect();
        playerManager.SetAttackSpeed(attackSpeedAmount);
    }
}
