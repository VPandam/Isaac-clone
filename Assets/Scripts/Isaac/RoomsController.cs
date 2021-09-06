using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 enum ExitSite
{
    up, down, right, left
};

public class RoomsController : MonoBehaviour { 
    
    public Room initialRoom;
    public List<Room> allRooms = new List<Room>();
    public List<Room> floorRooms = new List<Room>();
    Room currentRoom;
    Room newRoom;
    Vector3 startNewRoom;
    Vector3 correction;


    CameraController camera;
    public GameObject player;
    static public RoomsController instance;
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
        currentRoom.SetX(0); currentRoom.setY(0);

        
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
        newRoom = Instantiate(allRooms[1]);
        newRoom.transform.SetParent(this.transform, false);

        //0 = up, 1 = down, 2 = right, 3 = left
        int direction = Random.Range(0, 3);
        switch (direction)
        {
            case 0:
                startNewRoom = currentRoom.StartUp.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.StartDown.transform.position.x,
                startNewRoom.y - newRoom.StartDown.transform.position.y, 0);
                setNewRoomXY(newRoom, currentRoom.getX(), currentRoom.getY() + 1);
                break;
            case 1:
                startNewRoom = currentRoom.StartDown.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.StartUp.transform.position.x,
                startNewRoom.y - newRoom.StartUp.transform.position.y, 0);
                setNewRoomXY(newRoom, currentRoom.getX(), currentRoom.getY() - 1);
                break;
            case 2:
                startNewRoom = currentRoom.StartRight.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.StartLeft.transform.position.x,
                startNewRoom.y - newRoom.StartLeft.transform.position.y, 0);
                setNewRoomXY(newRoom, currentRoom.getX() +1, currentRoom.getY());
                break;
            case 3:
                startNewRoom = currentRoom.StartLeft.transform.position;
                correction = new Vector3(
                startNewRoom.x - newRoom.StartRight.transform.position.x,
                startNewRoom.y - newRoom.StartRight.transform.position.y, 0);
                setNewRoomXY(newRoom, currentRoom.getX() -1, currentRoom.getY() + 1);
                break;
        }


        newRoom.transform.position = correction;
        currentRoom = newRoom;
        Debug.Log(currentRoom.transform.position + " currentRoomTransformPos");
        //camera.MoveCameraTo(currentRoom.transform.position);
        


        
    }
    void InstantiateAllRooms ()
    {
        //for (int i = 0; i <= floorRooms.Count; i++)
        //{
        //    floorRooms.Add(allRooms[Random.Range(0, allRooms.Count - 1)]);         
        //}

        Debug.Log(floorRooms);

     

    }
    
    public void setNewRoomXY(Room room, int valueX, int valueY)
    {
        room.SetX(valueX); room.setY(valueY);
    }
}
