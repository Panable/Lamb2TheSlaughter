﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private List<Room> roomPrefabsToTryGenerate;

    //Generate every position of a current room
    //switch room

    public void Awake()
    {

    }

    /// <summary>
    /// Try and generate a room at a door with transform 'spawndoor' and roommanager 'rm'
    /// Cycle through every room/every orientation until it fits (shuffle these so we get random one everytime)
    /// return the generated room
    /// </summary>
    public GameObject GenerateRoom(Transform spawnDoor, RoomManager rm)
    {
        Vector3 direction;
        float distance;
        bool colliding;

        roomPrefabsToTryGenerate.Shuffle();
        while (true)
        {
            //if we have no room prefabs left to generate from return null
            if (roomPrefabsToTryGenerate.Count == 0)
                return null;

            colliding = false;

            //select first room in shuffled list to try
            Room currentRoom = roomPrefabsToTryGenerate[0];
            //find the possible door configurations of that room (orientations)
            List<Door> possibleDoorSpots = new List<Door>();
            currentRoom.doorInfo.ForEach(i => possibleDoorSpots.Add(i));

            //shuffle those orientations
            possibleDoorSpots.Shuffle();
            do
            {
                //if out of door orientations remove the room from list of rooms to try and go back to the start of the 1st while loop
                if (possibleDoorSpots.Count == 0) { roomPrefabsToTryGenerate.RemoveAt(0); break; }

                //spawn the first door orientation in list
                GameObject currentDoor = possibleDoorSpots[0].activateObj(spawnDoor.position, Quaternion.identity);
                currentDoor.transform.forward = spawnDoor.up;

                //check for collision
                foreach (RoomManager managers in ProceduralManager.roomsGenerated)
                {
                    bool inCollisionWithAGeneratedRoom = Physics.ComputePenetration(managers.roomCollider, ExtensionMethods.ColliderToWorldPoint(managers.roomCollider), managers.transform.rotation,
                possibleDoorSpots[0].collider, ExtensionMethods.ColliderToWorldPoint(possibleDoorSpots[0].collider), currentDoor.transform.rotation, out direction, out distance);
                    if (inCollisionWithAGeneratedRoom)
                    {
                        colliding = true;
                        break;
                    }
                }

                //if we are colliding remove door spot
                if (colliding)
                {
                    possibleDoorSpots.RemoveAt(0);

                    //if out of door orientations, remove door spot and go back to 1st while loop
                    if (possibleDoorSpots.Count == 0)
                    {
                        roomPrefabsToTryGenerate.RemoveAt(0);
                        break;
                    }
                }
                else //we succeeded in generating a room
                {
                    currentDoor = Instantiate(currentDoor);


                    //change parent room's door way to match the room generated
                    Quaternion rot = Quaternion.LookRotation(Vector3.up, currentDoor.transform.forward);
                    if (rm.spawnRoom)
                    {
                        rot = Quaternion.LookRotation(Vector3.up, currentDoor.transform.right);
                    }

                    Transform doorReplace = Instantiate<Transform>(currentRoom.GetDoorPrefab(), spawnDoor.position, rot, spawnDoor.transform.parent);
                    
                    //destory original doorway
                    Destroy(spawnDoor.gameObject);


                    return currentDoor;
                }

            } while (colliding);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateRoomPrefabs();
    }

    void InstantiateRoomPrefabs()
    {
        roomPrefabsToTryGenerate = new List<Room>();
        ProceduralManager.roomPrefabs.ForEach(i => roomPrefabsToTryGenerate.Add(i));
    }

    // Update is called once per frame
}
