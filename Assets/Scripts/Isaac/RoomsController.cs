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
    Room currentRoom;
    
    public Room CurrentRoom { get => currentRoom; set => currentRoom = value; }
    Room newRoom;

    Vector3 startNewRoom;
    Vector3 correction;

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

        StartCoroutine("InstantiateAllRooms");

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            NewRoom();
        }        
    }

    public void NewRoom()
    {
        Debug.Log("NewRoom");
        bool roomCreated = false;
        while (!roomCreated)
        {
            bool coordinateTaken = true;

            //0 = up, 1 = down, 2 = right, 3 = left
            int direction = Random.Range(0, 3);

            int newRoomX;
            int newRoomY;
            switch (direction)
            {
                case 0:
                    newRoomX = currentRoom.getX();
                    newRoomY = currentRoom.getY() + 1;

                    if (!CheckCoordinate(newRoomX, newRoomY))
                    {
                        coordinateTaken = false;
                        newRoom = Instantiate(allRooms[Random.Range(0, allRooms.Count)]);
                        newRoom.transform.SetParent(this.transform, false);

                        startNewRoom = currentRoom.StartUp.transform.position;
                        correction = new Vector3(
                        startNewRoom.x - newRoom.StartDown.transform.position.x,
                        startNewRoom.y - newRoom.StartDown.transform.position.y, 0);
                        setNewRoomXY(newRoom, newRoomX, newRoomY);
                    }
                    else
                    {
                        coordinateTaken = true;
                    }

                    break;
                case 1:
                    newRoomX = currentRoom.getX();
                    newRoomY = currentRoom.getY() - 1;

                    if (!CheckCoordinate(newRoomX, newRoomY))
                    {
                        coordinateTaken = false;
                        newRoom = Instantiate(allRooms[Random.Range(0, allRooms.Count)]);
                        newRoom.transform.SetParent(this.transform, false);

                        startNewRoom = currentRoom.StartDown.transform.position;
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
                    newRoomX = currentRoom.getX() + 1;
                    newRoomY = currentRoom.getY();

                    if (!CheckCoordinate(newRoomX, newRoomY))
                    {
                        coordinateTaken = false;
                        newRoom = Instantiate(allRooms[Random.Range(0, allRooms.Count)]);
                        newRoom.transform.SetParent(this.transform, false);

                        startNewRoom = currentRoom.StartRight.transform.position;
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
                    newRoomX = currentRoom.getX() - 1;
                    newRoomY = currentRoom.getY();

                    if (!CheckCoordinate(newRoomX, newRoomY))
                    {
                        coordinateTaken = false;
                        newRoom = Instantiate(allRooms[Random.Range(0, allRooms.Count)]);
                        newRoom.transform.SetParent(this.transform, false);

                        startNewRoom = currentRoom.StartLeft.transform.position;
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
    bool CheckCoordinate(int newRoomX, int newRoomY)
    {
        bool coordinateTaken = false;
        foreach (Room room in floorLoaded)
        {

            if (room.getX() == newRoomX && room.getY() == newRoomY)
            {
                coordinateTaken = true;
            }
        }
        return coordinateTaken;
    }
    IEnumerator InstantiateAllRooms()
    {
 
        for(int i = 0;  i < 5; i++)
        {
            NewRoom();
            yield return new WaitForSeconds(.2f);
        }

    }
    
    public void setNewRoomXY(Room room, int valueX, int valueY)
    {
        room.SetX(valueX); room.setY(valueY);
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
}
