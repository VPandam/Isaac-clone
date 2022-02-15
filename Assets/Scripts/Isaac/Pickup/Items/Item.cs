using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Pickup
{
    string effectDescription;
    public override void DoEffect()
    {
        base.DoEffect();
        Debug.Log("Item effect");

    }
}
