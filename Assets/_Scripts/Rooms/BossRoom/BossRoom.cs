using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    [SerializeField] GameObject stairsGO;
    protected override void Start()
    {
        base.Start();
        Debug.Log("BossRoomActivated");
    }

    public override void FinishRoom()
    {
        Invoke(nameof(SetStairsActive), 1);
        Invoke(nameof(OpenRoom), 1);
    }

    void SetStairsActive()
    {
        stairsGO.SetActive(true);
    }
}
