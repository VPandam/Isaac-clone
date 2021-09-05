using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject romOpen;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(romOpen, gameObject.transform);
        Instantiate(player, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
