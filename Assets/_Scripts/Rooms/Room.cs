using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

    [Serializable]
    public struct AudioSourceEnemyType
    {
        public AudioSource audioSource;
        public EnemyType enemyType;
    }
    public class Room : MonoBehaviour
    {
        public List<Enemy> enemies = new List<Enemy>();
        
        //Stores each audioSource of each enemy type.
        //This audioSources play a clip while there are enemies of their type alive.
        public List<AudioSourceEnemyType> audioSourceEnemyTypes = new List<AudioSourceEnemyType>();
        [HideInInspector]public List<Door> doors = new List<Door>();

        //Sets the door state
        [HideInInspector]public bool roomOpen;

        //True if the player is been already in the room.
        [HideInInspector]public bool playerEntered = false;

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

        public int x,y,ID;
        public RoomType roomType;

        AstarPath pathfinder;

        private GameObject LootRoomGO;

        //Minimap
        [SerializeField] GameObject minimapIcon;
        private Color minimapIconInitialColor;
        private Color activeRoomMinimapColor = new Color(115,115,115);
        private SpriteRenderer minimapIconSR;

        private bool roomLooted;
        [SerializeField] private GameObject roomLootGO;

        private Resources resources;
        //Audio
        private AudioSource roomAudioSource;
        [SerializeField] private AudioClip roomOpenClip, roomCloseClip;
        private void Awake()
        {
            // pathfinder.GetComponent<AstarPath>();
            minimapIconSR = minimapIcon.GetComponent<SpriteRenderer>();
        } 
        // Start is called before the first frame update
        protected virtual void Start()
        {
            
            if (enemies.Count == 0)
            {
                Invoke("SetExitZonesActive", 1f);
            }

            roomAudioSource = GetComponent<AudioSource>();
            resources = Resources.sharedInstance;
            
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
            resources.StartCoroutine(resources.PlayAudioClipDelayed(
                roomAudioSource, roomCloseClip, .5f, .4f));
            
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
            resources.StartCoroutine(resources.PlayAudioClipDelayed(
                roomAudioSource, roomOpenClip, .5f, .4f));

            foreach (Door door in doors)
            {
                door.SetOpen(true);
            }

        }
        public virtual void FinishRoom()
        {
            Invoke(nameof(SpawnRoomReward), 1);
            Invoke(nameof(OpenRoom), 1);
        }
        

        public void SetVisibleOnMinimap()
        {
            minimapIcon.SetActive(true);
            minimapIconInitialColor = minimapIconSR.color;
            minimapIconSR.color = activeRoomMinimapColor;
        }

        public void OnLeftRoom()
        {
            minimapIconSR.color = minimapIconInitialColor;
        }
        //Exit zones are triggers in the doors.
        public void SetExitZonesActive()
        {
            exitZones.SetActive(true);
        }

        public void SpawnRoomReward()
        {
            if(!roomLooted)
            {
                roomLooted = true;
                var roomLootList = ItemManager.instance.roomLoot;
                int randomIndex = Random.Range(0, roomLootList.Count);
                Instantiate(roomLootList[randomIndex], roomLootGO.transform.position, 
                    Quaternion.identity, Resources.sharedInstance.lootGO.transform);
            }
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


