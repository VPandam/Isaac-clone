using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : Item
{
    [SerializeField]private float moveSpeedAmmount;
    public override void DoEffect()
    {
        base.DoEffect();
        playerManager.moveSpeed += moveSpeedAmmount;
    }
}
