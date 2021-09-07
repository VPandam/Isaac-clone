using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public List<GameObject> doors = new List<GameObject>();
    public GameObject exitZones;
    public GameObject StartUp;
    public GameObject StartDown;
    public GameObject StartRight;
    public GameObject StartLeft;

    public int x;
    public int y;

    // Start is called before the first frame update
    void Start()
    {
       

        if (enemies.Count == 0)
        {
            Invoke("SetExitZonesActive", 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

     public void StartRoom()
    {
        

        CloseRoom();
        SpawnEnemies();
    }
    void CloseRoom()
    {
;
        if(doors.Count != 0)
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(true);
                door.GetComponent<Animator>().SetBool("EnemiesAlive", true);
            }
        }

    }
    void SpawnEnemies()
    {
        enemies.ForEach(x => { x.gameObject.SetActive(true); });

    }
    public void OpenRoom()
    {
        if (doors.Count != 0)
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Animator>().SetBool("EnemiesAlive", false);
                Destroy(door, 1f);
            }
            doors.Clear();
        }

    }
    
    //Exit zones are triggers in the doors, when activated generate a new room next to the exit zone.
    public void SetExitZonesActive()
    {
        exitZones.SetActive(true);
    }

    public int getX()
    {
        return x;
    }
    public int getY()
    {
        return y;
    }
    public void SetX(int value)
    {
        x = value;
    }
    public void setY(int value)
    {
        y = value;
    }
}
