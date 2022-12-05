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
        public ExitZone exitZoneUp;
        public ExitZone exitZoneDown;
        public ExitZone exitZoneRight;
        public ExitZone exitZoneLeft;

        //Position to spawn rooms
        public GameObject roomSpawnTop;
        public GameObject roomSpawnDown;
        public GameObject roomSpawnRight;
        public GameObject roomSpawnLeft;

        //Position to spawn player when he enter the room
        public GameObject playerSpawnTop;
        public GameObject playerSpawnDown;
        public GameObject playerSpawnRight;
        public GameObject playerSpawnLeft;

        //Position to spawn the doors
        public GameObject doorUpPos;
        public GameObject doorRightPos;
        public GameObject doorLeftPos;
        public GameObject doorDownPos;

        public int x;
        public int y;
        public int ID;
        public bool isGold;

        AstarPath pathfinder;

        private void Awake()
        {
            // pathfinder.GetComponent<AstarPath>();
        }
        // Start is called before the first frame update
        void Start()
        {
            if (enemies.Count == 0)
            {
                Invoke("SetExitZonesActive", 1f);

            }

        }
        public void FindDoors()
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


