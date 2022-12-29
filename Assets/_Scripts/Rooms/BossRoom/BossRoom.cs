using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    protected override void Start()
    {
        base.Start();
        Debug.Log("BossRoomActivated");
    }
}
