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
    public bool isGold;

    Room newxtRoomUp;
    Room nextRoomDown;
    Room nextRoomLeft;
    Room nextRoomRight;


    private void Awake()
    {
  
    }
    // Start is called before the first frame update
    void Start()
    {
        FindDoors();
        if (enemies.Count == 0)
        {
            Invoke("SetExitZonesActive", 1f);
            
        }
       
    }

    void FindDoors()
    {
        Door[] foundDoors = gameObject.GetComponentsInChildren<Door>();

        foreach (Door door in foundDoors)
        {
            doors.Add(door);
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
        
        
        foreach (Door door in doors)
        {
            
            door.SetCurrentState(DoorState.close);
        }
        

    }
    void SpawnEnemies()
    {
        enemies.ForEach(x => { x.gameObject.SetActive(true); });

    }
    public void OpenRoom()
    {

        foreach (Door door in doors)
        {
            door.SetCurrentState(DoorState.open);
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
    public int GetID()
    {
        return ID; ;
    }

    

}
