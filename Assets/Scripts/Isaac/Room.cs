using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public List<Door> doors = new List<Door>();
    public GameObject exitZones;
    public GameObject StartUp;
    public GameObject StartDown;
    public GameObject StartRight;
    public GameObject StartLeft;

    public GameObject doorUpPos;
    public GameObject doorRightPos;
    public GameObject doorLeftPos;
    public GameObject doorDownPos;

    public int x;
    public int y;
    public int ID;


    private void Awake()
    {
  
    }
    // Start is called before the first frame update
    void Start()
    {

        if (enemies.Count == 0)
        {
            Invoke("SetExitZonesActive", 1f);
        }

    }


     public void StartRoom()
    {
        if (enemies.Count != 0)
        {
            CloseRoom();
            SpawnEnemies();
        }
        
    }
    void CloseRoom()
    {

        if (doors.Count != 0)
        {
            foreach (Door door in doors)
            {
                door.SetCurrentState(DoorState.open);
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
            foreach (Door door in doors)
            {
                door.SetCurrentState(DoorState.open);
                
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

    public void SetID(int value)
    {
        ID = value;
    }
    public void GetID(int value)
    {
        ID = value;
    }

}
