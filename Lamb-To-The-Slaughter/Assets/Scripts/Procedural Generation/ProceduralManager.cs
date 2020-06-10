using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.Rendering;

public class ProceduralManager : MonoBehaviour
{
    public static bool activateDestroy = false;
    public static int numberOfLevelsToLoad = 3;
    public static bool procedurallyGenerating = true;
    public static int numberOfRoomsGenerated = 0;
    public static int maxRooms = 200;
    public static int maxDoorsPerRoom = 1;

    public static int roomLayer = 1 << 10;

    // Start is called before the first frame update

    public static List<Room> roomPrefabs = new List<Room>();
    public static GameObject doorPrefab;
    public static GameObject wallPrefab;
    public static GameObject spawnRoomPrefab;

    public static List<RoomManager> roomsGenerated = new List<RoomManager>();
    public static List<RoomManager> roomsToGenerate = new List<RoomManager>();

    public static MeshFilter doormesh;

    public bool startGeneration = true;

    private void Awake()
    {

        LoadRoomPrefabs();
        spawnRoomPrefab = Resources.Load<GameObject>("Prefabs/SpawnRoom");
        doorPrefab = Resources.Load<GameObject>("Prefabs/Room Assets/Doorway");
        wallPrefab = Resources.Load<GameObject>("Prefabs/Room Assets/Wall");
        doormesh = Resources.Load<MeshFilter>("doormesh");


    }

    void LoadRoomPrefabs()
    {
        for (int i = 0; i < numberOfLevelsToLoad; i++)
        {

            roomPrefabs.Add(new Room("Prefabs/Rooms/Room " + i.ToString()));
        }
        Debug.Log("room prefabs size is: " + roomPrefabs.Count);
    }

    /// <summary>
    /// Calculate the percentage of spawning a new door
    /// </summary>
    public static float DoorSpawnPercentage()
    {
        float percentage = 0.0f;
        //percentage = (roomsGenerated / maxRooms) * 100; 
        if (percentage >= 100)
            procedurallyGenerating = false;

        return percentage;
    }


    void Start()
    {
        if (startGeneration)
        {
            GameObject spawnRoom = Instantiate(spawnRoomPrefab, Vector3.zero, Quaternion.identity);
            RoomManager rm = spawnRoom.GetComponent<RoomManager>();
            roomsGenerated.Add(rm);
            roomsToGenerate.Add(rm);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ProceduralManager.numberOfRoomsGenerated <= ProceduralManager.maxRooms && roomsToGenerate.Count > 0)
        {
            Debug.Log("Trying to generate a new room");
            ProceduralManager.roomsToGenerate.Shuffle();
            ProceduralManager.roomsToGenerate[0].StartGeneration();
        } else {
            foreach (Room room in roomPrefabs)
            {
                room.DestroyAll();
            }
            Destroy(this);
        }

    }
}
