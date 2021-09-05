using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public GameObject []doors;
    public GameObject StartUp;
    public GameObject StartDown;
    public GameObject StartRight;
    public GameObject StartLeft;
    // Start is called before the first frame update
    void Start()
    {
       
        

        if (enemies.Count > 0)
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Animator>().SetBool("EnemiesAlive", true);
            }
            Invoke("StartRoom" , 1f);
        };
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void StartRoom()
    {
        CloseRoom();
        SpawnEnemies();
    }
    void CloseRoom()
    {
        Debug.Log(doors.Length);
        Debug.Log(enemies.Count);
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
    }
    void SpawnEnemies()
    {
        enemies.ForEach(x => { x.gameObject.SetActive(true); });

    }
    public void OpenRoom()
    {
        
        foreach (GameObject door in doors)
        {
            door.GetComponent<Animator>().SetBool("EnemiesAlive", false);
            Destroy(door, 1f);
        }
    }

}
