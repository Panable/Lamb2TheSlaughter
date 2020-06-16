using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.Rendering;

public class ProceduralManager : MonoBehaviour
{
    public static bool roomGenerating = false;
    [Header("Procedural Settings")]
    public static float numberOfRoomsToGenerate = 50.0f;
    /// <summary>
    /// Sometimes we want proceduralmanager without actually generating anything
    /// </summary>
    public bool startGeneration = true;
    public static int numberOfLevelsToLoad = 9;
    public static int maxDoorsPerRoom = 1;

    public static float numberOfRoomsGenerated = 0.0f;
    public static int roomLayer = 1 << 10;

    public static bool procedurallyGenerating = true;

    //A Database of all prefabs
    public static List<Room> roomPrefabs = new List<Room>();
    public static GameObject spawnRoomPrefab;
    public static Dictionary<char, Transform> doorPrefabs = new Dictionary<char, Transform>(); //Character corresponds to door prefab

    /// <summary>
    /// A database of all currently generated rooms. Used to check against collision
    /// </summary>
    public static List<RoomManager> roomsGenerated = new List<RoomManager>();

    /// <summary>
    /// A list of rooms still left to generate from
    /// </summary>
    public static List<RoomManager> roomsToGenerate = new List<RoomManager>();




    private void Awake()
    {

        LoadRoomPrefabs();
        spawnRoomPrefab = Resources.Load<GameObject>("Prefabs/SpawnRoom");
        LoadDoorPrefabs();


    }

    /// <summary>
    /// Load Room Prefabs from Resources/Prefabs/Rooms/Room _
    /// </summary>
    void LoadRoomPrefabs()
    {
        for (int i = 0; i < numberOfLevelsToLoad; i++)
        {

            roomPrefabs.Add(new Room("Prefabs/Rooms/Room " + i.ToString()));
        }
    }

    /// <summary>
    /// loading door prefab into dictionary from Resources/Prefabs/Rooms/Doors
    /// </summary>
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

    /// <summary>
    /// Begin formally procedurally generating.
    /// </summary>
    public void StartGeneration()
    {
        if (!startGeneration) return;

        //instantiate spawnroom
        GameObject spawnRoom = Instantiate(spawnRoomPrefab, Vector3.zero, Quaternion.identity);
        RoomManager rm = spawnRoom.GetComponent<RoomManager>();
        
        //add spawnroom's roommanager to our lists
        roomsGenerated.Add(rm);
        roomsToGenerate.Add(rm);

        //the generation continues in update after this!
    }

    void Start()
    {
        StartGeneration();
    }

    /// <summary>
    /// When procedural is done, destroy all objects to free up memory.
    /// </summary>
    private void KillProcedural()
    {
        foreach (Room room in roomPrefabs)
        {
            room.DestroyAll();
        }
        Destroy(this);
    }

    void Update()
    {
        bool stillNeedToGenerateRooms = ProceduralManager.numberOfRoomsGenerated <= ProceduralManager.numberOfRoomsToGenerate;
        bool stillRoomsToGenerate = roomsToGenerate.Count > 0;
        if (stillNeedToGenerateRooms && stillRoomsToGenerate && !roomGenerating)
        {
            //Shuffle the list, so we try to generate from a random room
            ProceduralManager.roomsToGenerate.Shuffle();

            ProceduralManager.roomsToGenerate[0].StartGeneration();
        }
        else
        {
            KillProcedural();
        }

    }
}
