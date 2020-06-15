using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    public bool spawnRoom = false;
    [SerializeField] bool forceGeneration = false;

    public bool triedGeneration = false;
    [SerializeField] public List<Transform> possibleDoorSpots = new List<Transform>();
    public List<GameObject> roomsGenerated = new List<GameObject>();
    public BoxCollider roomCollider;
    public RoomCollider roomColliderScript;
    private int numberOfDoorsToBeGenerated = 3;


    // Start is called before the first frame update

    private void Awake()
    {
        InstantiateDoorSpots();
        InstantiateCollider();
    }

    private void InstantiateDoorSpots()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "PossibleDoorLocation")
            {
                possibleDoorSpots.Add(child);
            }

        }
    }

    public BoxCollider InstantiateCollider()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "roomcollider")
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



    void Start()
    {
        if (ProceduralManager.numberOfRoomsGenerated >= ProceduralManager.maxRooms)
        {
            Destroy(this);
            return;
        }
    }

    public void StartGeneration()
    {
        InstantiateNumberOfDoorsToBeGenerated();
        RandomiseDoors();
        TryGenerateRooms();
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
            Destroy(this);
        }

    }

    /// <summary>
    /// Just going to randomise the door array here, so when we try and generate a door we're not doing it in order so it's more fun!
    /// </summary>
    void RandomiseDoors()
    {
        possibleDoorSpots.Shuffle();
    }

    void TryGenerateRooms()
    {

        for (int doorSpotIndex = 0; doorSpotIndex < numberOfDoorsToBeGenerated; doorSpotIndex++)
        {

            if (possibleDoorSpots.Count == 0 || ProceduralManager.numberOfRoomsGenerated >= ProceduralManager.maxRooms)
            {
                Debug.Log("no more doorspots to try");  
                // Destroy(this);
                return;
            }



            Transform currentDoorSpot = possibleDoorSpots[0];
            Debug.Log("trying " + possibleDoorSpots[0].name);
            if (!TryGenerateDoor(possibleDoorSpots[0]))
            {
                doorSpotIndex--;
            }

            possibleDoorSpots.RemoveAt(0);

        }
        Debug.Log("fin");
    }

    bool TryGenerateDoor(Transform currentDoorLocation)
    {

        RoomGenerator roomGenerator = currentDoorLocation.GetComponent<RoomGenerator>();

        if (roomGenerator.GenerateRoom(currentDoorLocation, this))
        {
            ProceduralManager.numberOfRoomsGenerated++;
            return true;
        }
        else
        {
            roomGenerator.InstantiateWall();
            return false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (forceGeneration)
            StartGeneration();
    }
}
