using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.AI;
using JetBrains.Annotations;
//using UnityEditor.PackageManager;
using System;

public class ProceduralManager : MonoBehaviour //Dhan
{
    public static GameObject spawnRoom;

    public const float CHEST_PER_SINGLEROOM = 0.5f;

    public static bool roomGenerating = false;
    [Header("Procedural Settings")]
    public static int numberOfRoomsToGenerate = 20;
    /// <summary>
    /// Sometimes we want proceduralmanager without actually generating anything
    /// </summary>
    public bool startGeneration = true;
    public static int numberOfLevelsToLoad = 9;
    public static int maxDoorsPerRoom = 1;

    public static int numberOfRoomsGenerated = 0;
    public static int roomLayer = 1 << 10;

    public static bool procedurallyGenerating = true;

    //A Database of all prefabs
    public static List<Room> roomPrefabs = new List<Room>();
    public static GameObject spawnRoomPrefab;
    public static GameObject bossRoomPrefab;
    public static Dictionary<char, Transform> doorPrefabs = new Dictionary<char, Transform>(); //Character corresponds to door prefab

    /// <summary>
    /// A database of all currently generated rooms. Used to check against collision
    /// </summary>
    public static List<RoomManager> roomsGenerated = new List<RoomManager>();

    /// <summary>
    /// A list of rooms still left to generate from
    /// </summary>
    public static List<RoomManager> roomsToGenerate = new List<RoomManager>();
    public GameObject player;
    [SerializeField] public BloodMessages bloodMessages;

    private void Awake()
    {

        LoadRoomPrefabs();

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

        spawnRoomPrefab = Resources.Load<GameObject>("Prefabs/SpawnRoom");
        bossRoomPrefab = Resources.Load<GameObject>("Prefabs/BossRoom/BossRoom");
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
        spawnRoom = Instantiate(spawnRoomPrefab, Vector3.zero, Quaternion.identity);
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
    void Update()
    {
        if (!procedurallyGenerating) return;

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
            procedurallyGenerating = false;
            Debug.Log("Kiling");

            KillProcedural();
            GenerateBossRoom();
            InstantiateAllDoorLocations();
            InstantiateChests();
            InstantiatePlanePrefabs();
            LoadingManager.EndLoadingBar();
            player.SetActive(true);
            bloodMessages.gameObject.SetActive(true);
        }

    }
    private void KillProcedural()
    {
        foreach (Room room in roomPrefabs)
        {
            room.DestroyAll();
        }

        //Destroy(this);

        /// roomsGenerated.CopyTo(surfaces)
        /// need to make unityAI.surfaces array be equal to the rooms generated/prefabs spawned.
    }

    private static void InstantiateAllDoorLocations()
    {
        foreach (RoomManager rm in roomsGenerated)
        {
            if (rm.spawnRoom) return;
            rm.InstantiateDoorLocations();
        }

    }

    private static void InstantiateChests()
    {
        List<RoomManager> singleEntranceRooms = new List<RoomManager>();
        foreach (RoomManager allRooms in roomsGenerated)
        {
            allRooms.InstantiateChests();
            if (allRooms.roomsGenerated.Count <= 0)
                singleEntranceRooms.Add(allRooms);
        }
        singleEntranceRooms.Shuffle();

        for (int i = 0; i < (singleEntranceRooms.Count * CHEST_PER_SINGLEROOM); i++)
        {
            singleEntranceRooms[i].chestLocations.Shuffle();
            singleEntranceRooms[i].chestLocations[0].gameObject.SetActive(true);
        }

    }

    public static GameObject bossroomcurrent;

    private static void GenerateBossRoom()
    {
        Vector3 spawnRoomPos = spawnRoom.transform.position;

        List<RoomManager> roommanager = new List<RoomManager>();
        List<float> distanceToSpawnRoom = new List<float>();

        foreach (RoomManager allRooms in roomsGenerated)
        {
            if (allRooms.spawnRoom) continue;
            float distance = allRooms.FindDistanceToSpawnRoom();
            roommanager.Add(allRooms);
            distanceToSpawnRoom.Add(distance);
        }
        IEnumerable<RoomManager> sortedByDistance = roommanager
              .Select((value, index) => new { Index = index, Value = value })
                .OrderBy(o => distanceToSpawnRoom[o.Index])
             .Select(o => o.Value);
        List<RoomManager> roomsSortedByDistance = sortedByDistance.ToList();
        roomsSortedByDistance.Reverse();

        bossroomcurrent = Instantiate(bossRoomPrefab);
        foreach (RoomManager currentRoom in roomsSortedByDistance)
        {
            foreach (Transform spot in currentRoom.possibleDoorSpots)
            {
                if (spot == null) break;
                if (spot.GetComponent<RoomGenerator>().GenerateBossRoom(bossroomcurrent, currentRoom))
                {
                    return;
                }

            }
        }
        Debug.Log("failed to find boss room");
    }

    public void InstantiatePlanePrefabs()
    {
        foreach (RoomManager room in roomsGenerated)
        {
            room.InstantiateGPSPlane();
        }
    }

}
