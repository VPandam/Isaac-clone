using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Level data")]
public class Level: ScriptableObject
{
    public int level;
    //List of all the room prefabs for this level.
    public List<Room> roomList = new List<Room>();
    //List of all the possible boss rooms for this level.
    public List<Room> BossRoomPrefabs = new List<Room>();

    //There should be only one gold room and one boss room per floor.
    public bool isGoldRoomLoaded = false;
    public bool isBossRoomLoaded = false;
    public bool isShopRoomLoaded = false;

    public Level GetLevelInstance()
    {
        return Instantiate(this);
    }
}
