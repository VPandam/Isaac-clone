using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsController : MonoBehaviour { 
    
    public GameObject initialRoom;
    public List<Room> allRooms = new List<Room>();
    Room currentRoom;
    Vector3 startNewRoom;

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

        Instantiate(initialRoom, gameObject.transform);
        Instantiate(player, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewRoom(string exit)
    {


        Debug.Log("Se llama a new room");
        currentRoom = Instantiate(allRooms[Random.Range(0, allRooms.Count - 1)]);
        currentRoom.transform.SetParent(this.transform, false);
        startNewRoom = currentRoom.StartRight.transform.position;

        Vector3 correction = new Vector3(
            startNewRoom.x - currentRoom.StartLeft.transform.position.x,
            startNewRoom.y - currentRoom.StartLeft.transform.position.y,
            0);
        currentRoom.transform.position = correction;
    }
    
}
