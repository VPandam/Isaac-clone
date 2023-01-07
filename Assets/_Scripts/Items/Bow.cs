using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Item
{
    [SerializeField] private float fireRateAmmount;
    public override void DoEffect()
    {
        base.DoEffect();
        playerManager.fireRate -= fireRateAmmount;
    }
}
