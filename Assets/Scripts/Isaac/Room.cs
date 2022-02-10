using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public List<Door> doors = new List<Door>();

    //Sets the door state
    public bool roomOpen;

    //True if the player is been already in the room.
    public bool playerEntered = false;

    //Exit zones are the door triggers
    public GameObject exitZones;


    public GameObject roomSpawnTop;
    public GameObject roomSpawnDown;
    public GameObject roomSpawnRight;
    public GameObject roomSpawnLeft;

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

        roomOpen = false;
        foreach (Door door in doors)
        {

            door.SetOpen(false);
        }


    }
    void SpawnEnemies()
    {
        enemies.ForEach(x => { x.gameObject.SetActive(true); });

    }
    public void OpenRoom()
    {
        roomOpen = true;
        foreach (Door door in doors)
        {
            door.SetOpen(true);
        }

    }

    //Exit zones are triggers in the doors.
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
