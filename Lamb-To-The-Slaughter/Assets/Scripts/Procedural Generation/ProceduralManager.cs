using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.Rendering;

public class ProceduralManager : MonoBehaviour
{
    public static bool activateDestroy = false;
    public static int numberOfLevelsToLoad = 7;
    public static bool procedurallyGenerating = true;
    public static int numberOfRoomsGenerated = 0;
    public static int maxRooms = 50;
    public static int maxDoorsPerRoom = 1;

    public static int roomLayer = 1 << 10;

    // Start is called before the first frame update

    public static List<Room> roomPrefabs = new List<Room>();
    public static GameObject doorPrefab;
    public static GameObject wallPrefab;
    public static GameObject spawnRoomPrefab;

    public static List<RoomManager> roomsGenerated = new List<RoomManager>();
    public static List<RoomManager> roomsToGenerate = new List<RoomManager>();
    public static Dictionary<char, Transform> doorPrefabs = new Dictionary<char, Transform>();

    public static MeshFilter doormesh;

    public bool startGeneration = true;


    private void Awake()
    {

        LoadRoomPrefabs();
        spawnRoomPrefab = Resources.Load<GameObject>("Prefabs/SpawnRoom");
        LoadDoorPrefabs();


    }

    void LoadRoomPrefabs()
    {
        for (int i = 0; i < numberOfLevelsToLoad; i++)
        {

            roomPrefabs.Add(new Room("Prefabs/Rooms/Room " + i.ToString()));
        }
        Debug.Log("room prefabs size is: " + roomPrefabs.Count);
    }


    void LoadDoorPrefabs()
    {
        string path = "Prefabs/Rooms/Doors";
        Transform[] fetchedDoors = Resources.LoadAll<Transform>(path);
        foreach (Transform door in fetchedDoors)
        {
            char[] nameOfDoor = door.name.ToCharArray();
            doorPrefabs.Add(nameOfDoor[0], door);
        }
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
