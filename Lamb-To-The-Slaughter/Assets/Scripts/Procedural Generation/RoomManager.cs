using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour //Dhan
{

    public Transform gpsPlane;
    public bool bossRoom;

    public Room currentRoom;
    //is this the spawnroom
    public bool spawnRoom = false;
    private int numberOfDoorsToBeGenerated = 3;

    public bool triedGeneration = false;

    public BoxCollider roomCollider;
    public RoomCollider roomColliderScript;

    public Transform entryDoor;
    GameObject player;
    ProceduralEnemySelection[] roomEnemySpawners;
    [SerializeField ]bool emptyRoom;

    /// <summary>
    /// Force this room to generate
    /// </summary>
    [SerializeField] bool forceGeneration = false;

    /// <summary>
    /// Children of this room (other rooms that have spawned from this room)
    /// </summary>
    public List<GameObject> roomsGenerated = new List<GameObject>();


    /// <summary>
    /// Possible door spots for this room
    /// </summary>
    public List<Transform> possibleDoorSpots = new List<Transform>();

    public List<Transform> doorLocations = new List<Transform>();

    public List<Transform> chestLocations = new List<Transform>();


    private void Awake()
    {
        FindSpawners();
        InstantiateDoorSpots();
        InstantiateCollider();
    }

    void FindSpawners()
    {
        roomEnemySpawners = GetComponentsInChildren<ProceduralEnemySelection>();
        foreach (ProceduralEnemySelection spawner in roomEnemySpawners)
        {
            if (spawner.Enemies.Length == 0)
            {
                emptyRoom = true;
            }
        }
    }

    public void InstantiateDoorLocations()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("DoorLocation"))
            {
                doorLocations.Add(child);
            }
        }
    }

    public void InstantiateChests()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Chest"))
            {
                chestLocations.Add(child);
                child.gameObject.SetActive(false);
            }
        }
    }

    public void InstantiateGPSPlane()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("GPSGraphic"))
            {
                gpsPlane = child;
                gpsPlane.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateGPSPlane()
    {
        InstantiateDoorLocations();
        gpsPlane.gameObject.SetActive(true);
        foreach (Transform door in doorLocations)
        {
            if (door.childCount == 1)
                continue;
            door.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void LockDoors()
    {
        InstantiateDoorLocations();
        foreach (Transform door in doorLocations)
        {
            if(!emptyRoom)
            {
                door.GetChild(0).gameObject.SetActive(true);
                door.GetChild(0).gameObject.GetComponent<Parasites>().DissolveIn();
                if (door.childCount > 1)
                    door.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                return;
            }
        }

    }

    public void UnlockDoors()
    {
        InstantiateDoorLocations();
        foreach (Transform door in doorLocations)
        {
            //Destroy(door.gameObject);
            door.GetChild(0).gameObject.GetComponent<Parasites>().DissolveOut();
            StartCoroutine(KillParasite(door.GetChild(0), 1f));
            //player.GetComponent<PlayerMovementCC>().canTeleport = true;
            //player = null;
        }
    }

    IEnumerator KillParasite(Transform door, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        door.gameObject.SetActive(false);
    }

    /// <summary>
    /// Get all the possible door spots and put them in a list
    /// </summary>
    private void InstantiateDoorSpots()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("PossibleDoorLocation"))
            {
                possibleDoorSpots.Add(child);
            }

        }
    }

    /// <summary>
    /// Initialize room collider
    /// </summary>
    public BoxCollider InstantiateCollider()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("roomcollider"))
            {
                roomCollider = child.GetComponent<BoxCollider>();
                roomColliderScript = child.GetComponent<RoomCollider>();
                return roomCollider;
            }
            else
            {
                continue;
            }
        }
        Debug.LogError("failed to find collider on " + transform.parent.name);
        return null;
    }

    public void FindEntryDoor()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "DoorLocation")
                entryDoor = child;
        }
    }

    /// <summary>
    /// Start Generation of current room
    /// </summary>
    public void StartGeneration()
    {
        ProceduralManager.roomGenerating = true;
        InstantiateNumberOfDoorsToBeGenerated();
        RandomiseDoors();
        TryGenerateRooms();
        ProceduralManager.roomGenerating = false;
        ProceduralManager.roomsToGenerate.Remove(this);
    }

    /// <summary>
    /// We have to find out how many doors we need to generate based on the amount of existing doors.
    /// </summary>
    void InstantiateNumberOfDoorsToBeGenerated()
    {
        if (ProceduralManager.numberOfRoomsGenerated == 0)
        {
            spawnRoom = true;
            numberOfDoorsToBeGenerated = 4;

        }
        else if (numberOfDoorsToBeGenerated == 0)
        {
            
        }

    }

    /// <summary>
    /// Just going to randomise the door array here, so when we try and generate a door we're not doing it in order so it's more fun!
    /// </summary>
    void RandomiseDoors()
    {
        possibleDoorSpots.Shuffle();
    }

    /// <summary>
    /// Try and generate # of rooms, cycling door spots if fail
    /// </summary>
    void TryGenerateRooms()
    {
        //generate amount of rooms 
        for (int doorSpotIndex = 0; doorSpotIndex < numberOfDoorsToBeGenerated; doorSpotIndex++)
        {

            if (possibleDoorSpots.Count == 0 || ProceduralManager.numberOfRoomsGenerated >= ProceduralManager.numberOfRoomsToGenerate)
                return;

            //try first doorspot 
            Transform currentDoorSpot = possibleDoorSpots[0];


            //if we fail at generating, deincrement the index so we still try and generate the precalculated number of rooms
            if (!TryGenerateDoor(possibleDoorSpots[0]))
            {
                doorSpotIndex--;
            }

            //remove the current doorspot as we have already generated there or failed at generating there
            possibleDoorSpots.RemoveAt(0);

        }
    }

    /// <summary>
    /// Begin generating doors at door location
    /// </summary>
    bool TryGenerateDoor(Transform currentDoorLocation)
    {
        RoomGenerator roomGenerator = currentDoorLocation.GetComponent<RoomGenerator>();

        //try and generate a room at the door location
        GameObject generatedRoom = roomGenerator.GenerateRoom(currentDoorLocation, this);

        //if succeeded add the room to proceduralmanager otherwise return false
        if (generatedRoom != null)
        {
            //set room name to room number
            generatedRoom.transform.name = ProceduralManager.numberOfRoomsGenerated.ToString();

            //add generated room to room manager list
            roomsGenerated.Add(generatedRoom.gameObject);

            //add room generated to procedural manager list
            ProceduralManager.roomsGenerated.Add(generatedRoom.GetComponent<RoomManager>());
            ProceduralManager.roomsToGenerate.Add(generatedRoom.GetComponent<RoomManager>());

            //increment number of rooms generated
            ProceduralManager.numberOfRoomsGenerated++;
            return true;
        }
        return false;
    }

    public float FindDistanceToSpawnRoom()
    {
        Vector3 roomPosition = transform.position;
        Vector3 spawnPosition = ProceduralManager.spawnRoom.transform.position;
        return Vector3.Distance(spawnPosition, roomPosition);
    }

    [SerializeField] bool destroyDoors = false;
    [SerializeField] bool activateChests = false;

    void Update()
    {
        if (activateChests)
        {
            InstantiateChests();
        }

        if (destroyDoors)
        {
            InstantiateDoorLocations();
            foreach (Transform door in doorLocations)
            {
                door.GetChild(0).gameObject.SetActive(true);
            }
        }

        if (forceGeneration)
            StartGeneration();
    }
}
