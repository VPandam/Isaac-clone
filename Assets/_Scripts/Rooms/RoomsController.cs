using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

enum ExitSite
    {
        up, down, right, left
    };

    public enum RoomType
    {
        Gold, DefaultRoom, Shop, Boss
    }

public enum NewRoomDirection
{
    Up, Down, Right, Left
}
    public class RoomsController : MonoBehaviour
    {
        //Singleton
        public static RoomsController _instance;
        [SerializeField] int amountOfInitialRooms = 10;
       
        //Prefabs of the rooms and doors
        [Header("Prefabs of the rooms and doors")]
        [SerializeField] Room initialRoom;
        [SerializeField] private Room goldRoomPrefab, shopRoomPrefab;
        [SerializeField] Door doorPrefab;
        [SerializeField] Door goldDoorPrefab;
        
        Level currentLevel;
        //Rooms already loaded
        private List<Room> roomsLoaded = new List<Room>();
        //The room we are working with
        private Room currentRoom;
        
        public GameObject RoomsParent;

        //Used on NewRoom method
        Room newRoom;
        //Used on NewRoom method
        Vector3 startNewRoom;
        
        //Used on NewRoom method
        //Position were we will move the new room
        Vector3 correction;
        int roomID = 0;
        
        //The farthest room from the initial one.
        private Room farthestRoom;
        
        CameraController cam;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }
        void Start()
        {
            cam = Camera.main.GetComponent<CameraController>();
            currentLevel = LevelManager._instance.GetLevel1();
        }

        public void CreateRooms()
        {
            currentRoom = Instantiate(initialRoom, gameObject.transform);
            roomsLoaded.Add(currentRoom);
            
            //Activates the minimap icon
            //This happens every time we enter in a room
            currentRoom.SetVisibleOnMinimap();
            
            PlayerManager.sharedInstance.currentRoom = currentRoom;
            currentRoom.SetX(0); currentRoom.setY(0);

            InstantiateAllRooms();
            foreach (Room room in roomsLoaded)
            {
                room.SetID(roomID);
                roomID++;
                InstantiateDoors(room);
            }
            ConnectDoors();
        }

        void InstantiateAllRooms()
        {
            for (int i = 0; i < amountOfInitialRooms; i++)
            {
                //Create a random room except boss room.
                Array values = Enum.GetValues(typeof(RoomType));
                RoomType randomRoomType = (RoomType)values.GetValue(Random.Range(0, values.Length-1));
                NewRoom(randomRoomType);
            }
            if (!currentLevel.isGoldRoomLoaded)
            {
                Debug.Log("NewRoomGold");
                NewRoom(RoomType.Gold);
            }
            if (!currentLevel.isShopRoomLoaded)
            {
                NewRoom(RoomType.Shop);
            }
            
            //Checks the farthest room position from the initial
            farthestRoom = CheckFarthestRoom();
            //Instantiate the boss room at the farthest position
            NewRoom(RoomType.Boss);

        }
        
        /// <summary>
        /// //Instantiate a room with a random prefab next to one of the loaded rooms. 
        /// </summary>
        /// <param name="roomType">Type of the room</param>
        public void NewRoom(RoomType roomType)
        {
            bool isRoomCreated = false;
            bool isRoomInPosition = false;
            newRoom = null;

            while (!isRoomInPosition)
            {
                //Get the reference of a random room already loaded in the level.
                Room randomLoadedRoom = roomsLoaded[Random.Range(0, roomsLoaded.Count)];

                //0 = up, 1 = down, 2 = right, 3 = left.
                NewRoomDirection roomDirection = (NewRoomDirection)Random.Range(0, 3);

                //Gets a random room from the list of the specific type.
                Room randomRoom = SelectRandomRoom(roomType);
                
                //InstantiateRoom[Direction] checks if there is a room in the coordinate pointed by direction.
                //If there is not a room instantiates a new room. If there is a room returns false.
                //We iterate over random directions until the room is created.
                switch (roomDirection)
                {
                    case NewRoomDirection.Up:
                        //There are some rooms that can't be spawned at certain directions.
                        //If the room can't be spawned up, it won't have a doorDownPos.
                        if (randomRoom.doorDownPos != null)
                        {
                            newRoom = randomRoom;
                            if(roomType == RoomType.Boss) isRoomCreated = InstantiateRoomTop(farthestRoom);
                            else isRoomCreated = InstantiateRoomTop(randomLoadedRoom);
                        }
                        break;
                    case NewRoomDirection.Down:
                        if (randomRoom.doorUpPos != null)
                        {
                            newRoom = randomRoom;
                            if(roomType == RoomType.Boss) isRoomCreated = InstantiateRoomDown(farthestRoom);
                            else isRoomCreated = InstantiateRoomDown(randomLoadedRoom);
                        }
                        break;
                    case NewRoomDirection.Right:
                        if (randomRoom.doorLeftPos != null)
                        {
                            newRoom = randomRoom;
                            if(roomType == RoomType.Boss) isRoomCreated = InstantiateRoomRight(farthestRoom);
                            else isRoomCreated = InstantiateRoomRight(randomLoadedRoom);
                        }
                        break;
                    case NewRoomDirection.Left:
                        if (randomRoom.doorRightPos != null)
                        {
                            newRoom = randomRoom;
                            if(roomType == RoomType.Boss) isRoomCreated = InstantiateRoomLeft(farthestRoom);
                            else isRoomCreated = InstantiateRoomLeft(randomLoadedRoom);
                        }
                        break;
                }

                //If the new room is created, set its position.
                if (isRoomCreated)
                {
                    newRoom.transform.position = correction;
                    currentRoom = newRoom;
                    roomsLoaded.Add(currentRoom);
                    isRoomInPosition = true;
                    if (newRoom.roomType == RoomType.Gold) currentLevel.isGoldRoomLoaded = true;
                    else if (newRoom.roomType == RoomType.Shop) currentLevel.isShopRoomLoaded = true;
                    else if (newRoom.roomType == RoomType.Boss) currentLevel.isBossRoomLoaded = true;
                }
            }
        }


        /// <summary>
        /// Selects a random room from the specified room type list.
        /// </summary>
        /// <param name="roomType"> The type of the room </param>
        /// <returns> A reference to a random room from the specified roomType list </returns>
        Room SelectRandomRoom(RoomType roomType)
        {
            Room roomToInstantiate = null;
            while (roomToInstantiate == null)
            {
                switch (roomType)
                {
                    case RoomType.Boss:
                        var bossRoomList = currentLevel.BossRoomPrefabs;
                        roomToInstantiate = bossRoomList[Random.Range(0, bossRoomList.Count)];
                        return roomToInstantiate;
                    
                    case RoomType.Gold:
                        //There should be only one gold room per level.
                        //If the gold room is been not loaded yet, instantiate the gold room. 
                        //Else restart the loop.
                        if (!currentLevel.isGoldRoomLoaded)
                        {
                            roomToInstantiate = goldRoomPrefab;
                            return roomToInstantiate;
                        }else roomType = RoomType.DefaultRoom;
                        break;
                    
                    case RoomType.Shop:
                        //There should be only one shop room per level.
                        //If the shop room is been not loaded yet, instantiate the shop room. 
                        //Else restart the loop.
                        if (!currentLevel.isShopRoomLoaded)
                        {
                            roomToInstantiate = shopRoomPrefab;
                            return roomToInstantiate;
                        } else roomType = RoomType.DefaultRoom;
                        break;
                    case RoomType.DefaultRoom:
                        //Instantiate a normal room.
                        var roomsList = currentLevel.roomList;
                        roomToInstantiate = roomsList[Random.Range(0, roomsList.Count)];
                        return roomToInstantiate;
                }
            }
            return null;
        }
        #region "Methods for checking coordinates up, down, right and left."

        /// <summary>
        /// Checks if the the coordinate in the top of the base room is free.
        /// If is free, instantiate a random room, set the vector correction and return true.
        /// Else, return false.
        /// </summary>
        /// 
        /// <param name="baseRoom"> The base room </param>
        /// <param name="isGoldRoom"> If true, the room will be gold. </param>
        /// <returns>True if the room is created or false if the coordinate is not free</returns>
        bool  InstantiateRoomTop(Room baseRoom )
        {
            //Get the base room coordinates
            int baseRoomX = baseRoom.getX();
            int baseRoomY = baseRoom.getY();

            //Create the new room coordinates.
            int newRoomX = baseRoomX;
            int newRoomY = baseRoomY + 1;

            //If there is not a room in the given coordinate
            if (!CheckCoordinate(newRoomX, newRoomY)[0])
            {
                newRoom = Instantiate(newRoom, Vector3.zero, newRoom.transform.rotation,
                    RoomsParent.transform);
                //Set the coordinates of the new room.
                setNewRoomXY(newRoom, newRoomX, newRoomY);

                startNewRoom = baseRoom.roomSpawnTop.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.roomSpawnDown.transform.position.x,
                startNewRoom.y - newRoom.roomSpawnDown.transform.position.y, 0);
                return true;
            }
            else
                return false;

        }
        /// <summary>
        /// Checks if the the coordinate in the bottom side of the base room is free.
        /// If is free, instantiate a random room, set the vector correction and return true.
        /// Else, return false.
        /// </summary>

        /// <param name="baseRoom"> The base room </param>
        /// <param name="isGoldRoom"> If true, the room will be gold. </param>
        /// <returns>True if the room is created or false if the coordinate is not free</returns>
        bool InstantiateRoomDown(Room baseRoom)
        {
            //Get the base room coordinates
            int baseRoomX = baseRoom.getX();
            int baseRoomY = baseRoom.getY();

            //Create the new room coordinates.
            int newRoomX = baseRoomX;
            int newRoomY = baseRoomY - 1;

            if (!CheckCoordinate(newRoomX, newRoomY)[0])
            {
                newRoom = Instantiate(newRoom, Vector3.zero, newRoom.transform.rotation,
                    RoomsParent.transform);
                //Set the coordinates of the new room.
                setNewRoomXY(newRoom, newRoomX, newRoomY);

                //Correction is the vector were we will move our new room.
                startNewRoom = baseRoom.roomSpawnDown.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.roomSpawnTop.transform.position.x,
                startNewRoom.y - newRoom.roomSpawnTop.transform.position.y, 0);
                return true;
            }
            else
                return false;

        }
        /// <summary>
        /// Checks if the the coordinate in the right of the base room is free.
        /// If is free, instantiate a random room, set the vector correction and return true.
        /// Else, return false.
        /// </summary>

        /// <param name="baseRoom"> The base room </param>
        /// <param name="isGoldRoom"> If true, the room will be gold. </param>
        /// <returns>True if the room is created or false if the coordinate is not free</returns>
        bool InstantiateRoomRight(Room baseRoom)
        {
            //Get the base room coordinates
            int baseRoomX = baseRoom.getX();
            int baseRoomY = baseRoom.getY();

            //Create the new room coordinates.
            int newRoomX = baseRoomX + 1;
            int newRoomY = baseRoomY;

            if (!CheckCoordinate(newRoomX, newRoomY)[0])
            {
                newRoom = Instantiate(newRoom, Vector3.zero, newRoom.transform.rotation,
                    RoomsParent.transform);
                //Set the coordinates of the new room.
                setNewRoomXY(newRoom, newRoomX, newRoomY);

                //Correction is the vector were we will move our new room.
                startNewRoom = baseRoom.roomSpawnRight.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.roomSpawnLeft.transform.position.x,
                startNewRoom.y - newRoom.roomSpawnLeft.transform.position.y, 0);
                return true;
            }
            else
                return false;

        }
        /// <summary>
        /// Checks if the the coordinate in the left of the base room is free.
        /// If is free, instantiate a random room, set the vector correction and return true.
        /// Else, return false.
        /// </summary>

        /// <param name="baseRoom"> The base room </param>
        /// <returns>True if the room is created or false if the coordinate is not free</returns>
        bool InstantiateRoomLeft(Room baseRoom)
        {
            //Get the base room coordinates
            int baseRoomX = baseRoom.getX();
            int baseRoomY = baseRoom.getY();

            //Create the new room coordinates.
            int newRoomX = baseRoomX - 1;
            int newRoomY = baseRoomY;

            if (!CheckCoordinate(newRoomX, newRoomY)[0])
            {
                newRoom = Instantiate(newRoom, Vector3.zero, newRoom.transform.rotation,
                    RoomsParent.transform);
                //Set the coordinates of the new room.
                setNewRoomXY(newRoom, newRoomX, newRoomY);

                //Correction is the vector were we will move our new room.
                startNewRoom = baseRoom.roomSpawnLeft.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.roomSpawnRight.transform.position.x,
                startNewRoom.y - newRoom.roomSpawnRight.transform.position.y, 0);
                return true;

            }
            else
                return false;

        }
        #endregion

        /// <summary>
        ///Checks if the given coordinate is taken and tells if it is a gold room.
        /// </summary>
        /// <param name="newRoomX"></param>
        /// <param name="newRoomY"></param>
        /// <returns> Returns an array of 2 bools. 
        /// First one is true if there is a room in the given coordinate.
        ///  Second one is true if it's a gold room.
        /// </returns>
        bool[] CheckCoordinate(int newRoomX, int newRoomY)
        {
            bool coordinateTaken = false;
            bool isGold = false;
            foreach (Room room in roomsLoaded)
            {
                if (room.getX() == newRoomX && room.getY() == newRoomY)
                {
                    coordinateTaken = true;
                    if (room.roomType == RoomType.Gold)
                    {
                        isGold = true;
                    }
                }
            }
            bool[] values = { coordinateTaken, isGold };
            return values;
        }

        /// <summary>
        /// Check if there are rooms around the current room and instantiate a door per room.
        /// </summary>
        /// <param name="room"> The current room </param>
        void InstantiateDoors(Room room)
        {
            bool[] values;

            //If there's a room in the right side of the current room instantiate a door
            values = CheckCoordinate(room.x + 1, room.y);
            if (values[0])
            {
                InstantiateRightDoor(room, values[1]);
            }

            //If there's a room in the left side of the current room instantiate a door
            values = CheckCoordinate(room.x - 1, room.y);
            if (values[0])
            {
                InstantiateLeftDoor(room, values[1]);
            }

            //If there's a room in the top side of the current room instantiate a door
            values = CheckCoordinate(room.x, room.y + 1);
            if (values[0])
            {
                InstantiateTopDoor(room, values[1]);
            }

            //If there's a room in the down side of the current room instantiate a door
            values = CheckCoordinate(room.x, room.y - 1);
            if (values[0])
            {
                InstantiateDownDoor(room, values[1]);
            }
        }

        #region Instantiate each door methods
        void InstantiateRightDoor(Room room, bool isGoldRoom)
        {
            GameObject doorPosition = room.doorRightPos;

            //Disable the collider used as a substitute of the door. 
            var doorCollider = doorPosition.GetComponent<BoxCollider2D>();
            doorCollider.enabled = false;

            //If the door is in a side of the room, rotates the door prefab 90 degrees.
            Quaternion doorAngle = Quaternion.AngleAxis(90f, Vector3.forward);

            Door door;
            if (doorPosition)
            {
                //If the room is a gold one, instantiate a goldDoor prefab, else instantiate a normal door prefab. 
                if (isGoldRoom)
                {
                    door = Instantiate(goldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                    door.doorPos = DoorPos.Right;
                }
                else
                {
                    door = Instantiate(doorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                    door.doorPos = DoorPos.Right;
                }
            }
        }
        void InstantiateLeftDoor(Room room, bool isGoldRoom)
        {
            GameObject doorPosition = room.doorLeftPos;

            //Disable the collider used as a substitute of the door. 
            doorPosition.GetComponent<BoxCollider2D>().enabled = false;

            //If the door is in a side of the room, rotates the door prefab 90 degrees.
            Quaternion doorAngle = Quaternion.AngleAxis(90f, Vector3.forward);

            Door door;
            if (isGoldRoom)
            {
                door = Instantiate(goldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                door.doorPos = DoorPos.Left;
            }
            else
            {
                door = Instantiate(doorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                door.doorPos = DoorPos.Left;
            }
        }
        void InstantiateTopDoor(Room room, bool isGoldRoom)
        {
            GameObject doorPosition = room.doorUpPos;

            //Disable the collider used as a substitute of the door. 
            doorPosition.GetComponent<BoxCollider2D>().enabled = false;

            //If the door is in a side of the room, rotates the door prefab 90 degrees.
            Quaternion doorAngle = Quaternion.identity;

            Door door;
            if (isGoldRoom)
            {
                door = Instantiate(goldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                door.doorPos = DoorPos.Up;
            }
            else
            {
                door = Instantiate(doorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                door.doorPos = DoorPos.Up;
            }
        }
        void InstantiateDownDoor(Room room, bool isGoldRoom)
        {
            GameObject doorPosition = room.doorDownPos;

            //Disable the collider used as a substitute of the door. 
            doorPosition.GetComponent<BoxCollider2D>().enabled = false;

            //If the door is in a side of the room, rotates the door prefab 90 degrees.
            Quaternion doorAngle = Quaternion.identity;

            Door door;
            if (isGoldRoom)
            {
                door = Instantiate(goldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                door.doorPos = DoorPos.Down;
            }
            else
            {
                door = Instantiate(doorPrefab, doorPosition.transform.position, doorAngle, room.transform);
                door.doorPos = DoorPos.Down;
            }
        }

        #endregion

        /// <summary>
        /// Once all doors are instantiated, set connections between them.
        /// </summary>
        void ConnectDoors()
        {
            foreach (var room in roomsLoaded)
            {
                room.FindDoors();
                foreach (var door in room.doors)
                {
                    SetDoorSpawnPos(door, room);
                }
            }
        }

        /// <summary>
        /// Set the spawn position of the player when crossing a specific door.
        /// </summary>
        /// <param name="door">The door</param>
        /// <param name="room">The room where the door is</param>
        void SetDoorSpawnPos(Door door, Room room)
        {
            Room roomToConnect;
            //Depending on the door position, find the room to connect with and set the player spawn position.
            //This variable is stored in each exit zone of each door.
            switch (door.doorPos)
            {
                case DoorPos.Up:
                    roomToConnect = roomsLoaded.Find(loadedRoom => loadedRoom.x == room.x && loadedRoom.y == room.y + 1);
                    room.exitZoneUp.playerSpawnPosition = roomToConnect.playerSpawnDown.transform.position;
                    room.exitZoneUp.roomToSpawn = roomToConnect;
                    break;
                case DoorPos.Down:
                    roomToConnect = roomsLoaded.Find(loadedRoom => loadedRoom.x == room.x && loadedRoom.y == room.y - 1);
                    room.exitZoneDown.playerSpawnPosition = roomToConnect.playerSpawnTop.transform.position;
                    room.exitZoneDown.roomToSpawn = roomToConnect;
                    break;
                case DoorPos.Right:
                    roomToConnect = roomsLoaded.Find(loadedRoom => loadedRoom.x == room.x + 1 && loadedRoom.y == room.y);
                    room.exitZoneRight.playerSpawnPosition = roomToConnect.playerSpawnLeft.transform.position;
                    room.exitZoneRight.roomToSpawn = roomToConnect;
                    break;
                case DoorPos.Left:
                    roomToConnect = roomsLoaded.Find(loadedRoom => loadedRoom.x == room.x - 1 && loadedRoom.y == room.y);
                    room.exitZoneLeft.playerSpawnPosition = roomToConnect.playerSpawnRight.transform.position;
                    room.exitZoneLeft.roomToSpawn = roomToConnect;
                    break;

                default:
                    break;
            }
        }

        Room CheckFarthestRoom()
        {
            float maxDistance = 0;
            Room farthestRoom = null;
            foreach (var room in roomsLoaded)
            {
                float distance = Vector2.Distance(roomsLoaded.ElementAt(0).transform.position, room.transform.position);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestRoom = room;
                }
            }

            return farthestRoom;
        }
        public void setNewRoomXY(Room room, int valueX, int valueY)
        {
            room.SetX(valueX); room.setY(valueY);
        }

        public void LoadNewLevel()
        {
            currentLevel = LevelManager._instance.GetNextLevel(currentLevel);
            DestroyAllRooms();
            CreateRooms();
        }
        
        void DestroyAllRooms()
        {
            foreach (var room in roomsLoaded)
            {
                Destroy(room.gameObject);
            }
            roomsLoaded.Clear();
            roomID = 0;
        }
    }




