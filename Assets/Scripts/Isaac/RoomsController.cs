using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 enum ExitSite
{
    up, down, right, left
};

public class RoomsController : MonoBehaviour {
    
    static public RoomsController instance;

    public Room initialRoom;
    public List<Room> allRooms = new List<Room>();
    public List<Room> floorRooms = new List<Room>();
    private List<Room> floorLoaded = new List<Room>();
    private Room currentRoom;
    public Door doorPrefab;
    public Door upDoorPrefab;
    public Door goldDoorPrefab;
    public Door upGoldDoorPrefab;

    bool instantiatedGoldRoom = false;

    
    Room newRoom;

    Vector3 startNewRoom;
    Vector3 correction;
    int roomID = 0;


    CameraController camera;
    public GameObject player;
  
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        Instantiate(player, gameObject.transform);
        camera = Camera.main.GetComponent<CameraController>();

        currentRoom = Instantiate(initialRoom, gameObject.transform);
        floorLoaded.Add(currentRoom);
        currentRoom.SetX(0); currentRoom.setY(0);

        InstantiateAllRooms();
        foreach (Room room in floorLoaded)
        {
            room.SetID(roomID);
            roomID++;
            InstantiateDoors(room);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
     
    
  
    }
    public void NewRoom(bool goldRoom)
    {

        Debug.Log("NewRoom");
        bool roomCreated = false;
        while (!roomCreated)
        {
            bool coordinateTaken = true;


            //0 = up, 1 = down, 2 = right, 3 = left
            int direction = Random.Range(0, 3);
            Room randomLoadedRoom = floorLoaded[Random.Range(0, floorLoaded.Count)];

            int newRoomX;
            int newRoomY;
            switch (direction)
            {
                case 0:
                    newRoomX = randomLoadedRoom.getX();
                    newRoomY = randomLoadedRoom.getY() + 1;

                    if (!CheckCoordinate(newRoomX, newRoomY)[0])
                    {
                        coordinateTaken = false;
                        if (goldRoom){newRoom = InstantiateRoom(true);
                        }else { newRoom = InstantiateRoom(false); }
                        
                        //newRoom.transform.SetParent(this.transform, false);
                        setNewRoomXY(newRoom, newRoomX, newRoomY);

                        startNewRoom = randomLoadedRoom.StartUp.transform.position;
                        correction = new Vector3(
                        startNewRoom.x - newRoom.StartDown.transform.position.x,
                        startNewRoom.y - newRoom.StartDown.transform.position.y, 0);

                    }
                    else
                    {
                        coordinateTaken = true;
                    }

                    break;
                case 1:

                    newRoomX = randomLoadedRoom.getX();
                    newRoomY = randomLoadedRoom.getY() - 1;


                    if (!CheckCoordinate(newRoomX, newRoomY)[0])
                    {
                        coordinateTaken = false;
                        if (goldRoom){newRoom = InstantiateRoom(true);
                        }else { newRoom = InstantiateRoom(false); }
                        //newRoom.transform.SetParent(this.transform, false);

                        startNewRoom = randomLoadedRoom.StartDown.transform.position;
                        correction = new Vector3(
                        startNewRoom.x - newRoom.StartUp.transform.position.x,
                        startNewRoom.y - newRoom.StartUp.transform.position.y, 0);
                        setNewRoomXY(newRoom, newRoomX, newRoomY);
                    }
                    else
                    {
                        coordinateTaken = true;
                    }

                    break;
                case 2:
                    newRoomX = randomLoadedRoom.getX() + 1;
                    newRoomY = randomLoadedRoom.getY();


                    if (!CheckCoordinate(newRoomX, newRoomY)[0])
                    {

                        coordinateTaken = false;
                        if (goldRoom){newRoom = InstantiateRoom(true);
                        }else { newRoom = InstantiateRoom(false); }
                        //newRoom.transform.SetParent(this.transform, false);

                        startNewRoom = randomLoadedRoom.StartRight.transform.position;
                        correction = new Vector3(
                        startNewRoom.x - newRoom.StartLeft.transform.position.x,
                        startNewRoom.y - newRoom.StartLeft.transform.position.y, 0);
                        setNewRoomXY(newRoom, newRoomX, newRoomY);
                    }
                    else
                    {
                        coordinateTaken = true;
                    }

                    break;
                case 3:
                    newRoomX = randomLoadedRoom.getX() - 1;
                    newRoomY = randomLoadedRoom.getY();


                    if (!CheckCoordinate(newRoomX, newRoomY)[0])
                    {
                        coordinateTaken = false;
                        if (goldRoom){newRoom = InstantiateRoom(true);
                        }else { newRoom = InstantiateRoom(false); }
                        //newRoom.transform.SetParent(this.transform, false);

                        startNewRoom = randomLoadedRoom.StartLeft.transform.position;
                        correction = new Vector3(
                        startNewRoom.x - newRoom.StartRight.transform.position.x,
                        startNewRoom.y - newRoom.StartRight.transform.position.y, 0);
                        setNewRoomXY(newRoom, newRoomX, newRoomY);
                    }
                    else
                    {
                        coordinateTaken = true;
                    }
                    break;
            }


            if (!coordinateTaken)
            {
                newRoom.transform.position = correction;
                currentRoom = newRoom;

                floorLoaded.Add(currentRoom);
                roomCreated = true;
                Debug.Log(currentRoom.transform.position + " currentRoomTransformPos");
            }
        }






    }
    Room InstantiateRoom(bool goldRoom)
    {
        bool instantiatedRoom = false;
        
        while (instantiatedRoom == false)
        {
            Room roomToInstantiate;

            if (goldRoom)
            { roomToInstantiate = allRooms[2];
            } else {roomToInstantiate = allRooms[Random.Range(0, allRooms.Count)]; }          

            if (roomToInstantiate.isGold == true)
            {
                if (instantiatedGoldRoom == false)
                {
                    instantiatedGoldRoom = true;
                    instantiatedRoom = true;
                    return Instantiate(roomToInstantiate);
                }
            }
            else
            {
                instantiatedRoom = true;
                return Instantiate(roomToInstantiate);
            }
 
        }
        return null;
        
    }
    bool[] CheckCoordinate(int newRoomX, int newRoomY)
    {
        bool coordinateTaken = false;
        bool isGold = false;
        foreach (Room room in floorLoaded)
        {

            if (room.getX() == newRoomX && room.getY() == newRoomY)
            {
                coordinateTaken = true;
                if (room.isGold){
                    isGold = true; 
                }


            }
        }
        bool[] values = { coordinateTaken, isGold };
        return values;
    }

    //Check if there are rooms around the current room and instance door per room.
    void InstantiateDoors(Room room)
    {
        bool[] values;

        values = CheckCoordinate(room.x + 1, room.y);
        if (values[0])
        {
            if (values[1]) { Instantiate(goldDoorPrefab, room.doorRightPos.transform.position, Quaternion.AngleAxis(90f, Vector3.forward), room.transform); }
            else
            {
                Instantiate(doorPrefab, room.doorRightPos.transform.position, Quaternion.AngleAxis(90f, Vector3.forward), room.transform);
            }
        }

        values = CheckCoordinate(room.x - 1, room.y);
        if (values[0])
        {
            if (values[1]) { Instantiate(goldDoorPrefab, room.doorLeftPos.transform.position, Quaternion.AngleAxis(90f, Vector3.forward), room.transform); }
            else
            {
                Instantiate(doorPrefab, room.doorLeftPos.transform.position, Quaternion.AngleAxis(90f, Vector3.forward), room.transform);
            }
        }

        values = CheckCoordinate(room.x, room.y + 1);
        if (values[0])
        {
            if (values[1]) { Instantiate(upGoldDoorPrefab, room.doorUpPos.transform.position, Quaternion.identity, room.transform); }
            else
            {
                Instantiate(upDoorPrefab, room.doorUpPos.transform.position, Quaternion.identity, room.transform);
            }
        }


        values = CheckCoordinate(room.x, room.y - 1);
        if (values[0])
        {
            if (values[1]) { Instantiate(goldDoorPrefab, room.doorDownPos.transform.position, Quaternion.identity, room.transform); }
            else
            {
                Instantiate(doorPrefab, room.doorDownPos.transform.position, Quaternion.identity, room.transform);
            }
        }
    }
    void InstantiateAllRooms()
    {
 
        for(int i = 0;  i < 10; i++)
        {
            NewRoom(false);
            
        }

        if (!instantiatedGoldRoom)
        {
            NewRoom(true);
        }
            
    }
    
    public void setNewRoomXY(Room room, int valueX, int valueY)
    {
        room.SetX(valueX); room.setY(valueY);
    }


}
