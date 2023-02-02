using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    [SerializeField] GameObject stairsGO;


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
