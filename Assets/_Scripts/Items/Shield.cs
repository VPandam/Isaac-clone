using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Item
{
    public override void DoEffect()
    {
        base.DoEffect();
        playerManager.UpdateHp(6, HpType.Blue);
    }
}
