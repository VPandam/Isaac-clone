using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTear : Tear
{
    public override void Start()
    {
        base.Start();
        Invoke("DestroyTear", PlayerManager.sharedInstance.attackRange);
    }
}
