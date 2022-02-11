using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Pickup
{
    float attackSpeed;
    int hp;
    int range;
    public override void DoEffect()
    {
        base.DoEffect();
        Debug.Log("Item effect");

    }
}
