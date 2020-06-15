﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomManager : MonoBehaviour
{
    //is this the spawnroom
    public bool spawnRoom = false;
    private int numberOfDoorsToBeGenerated = 3;

    public bool triedGeneration = false;

    public BoxCollider roomCollider;
    public RoomCollider roomColliderScript;

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
    [SerializeField] public List<Transform> possibleDoorSpots = new List<Transform>();

    private void Awake()
    {
        InstantiateDoorSpots();
        InstantiateCollider();
    }

    /// <summary>
    /// Get all the possible door spots and put them in a list
    /// </summary>
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

    /// <summary>
    /// Initialize room collider
    /// </summary>
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
        if (ProceduralManager.numberOfRoomsGenerated >= ProceduralManager.numberOfRoomsToGenerate)
        {
            Destroy(this);
            return;
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

        GameObject generatedRoom = roomGenerator.GenerateRoom(currentDoorLocation, this);

        if (generatedRoom != null)
        {
            generatedRoom.transform.name = ProceduralManager.numberOfRoomsGenerated.ToString() + " from " + currentDoorLocation.parent.transform.name;
            roomsGenerated.Add(generatedRoom.gameObject);


            ProceduralManager.roomsGenerated.Add(generatedRoom.GetComponent<RoomManager>());
            ProceduralManager.roomsToGenerate.Add(generatedRoom.GetComponent<RoomManager>());

            ProceduralManager.numberOfRoomsGenerated++;
            return true;
        }
        return false;
    }



    // Update is called once per frame
    void Update()
    {
        if (forceGeneration)
            StartGeneration();
    }
}
