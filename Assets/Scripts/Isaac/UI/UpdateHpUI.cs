using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHpUI : MonoBehaviour
{
    public GameObject hpGO;
    public GameObject hpContainer;

    PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = PlayerStats.instance;
        if (hpGO && hpContainer && playerStats)
        {
            for (int i = 0; i < playerStats.MaxHealth / 2; i++)
            {
                GameObject hp = Instantiate(hpGO);
                hp.transform.SetParent(hpContainer.transform);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
