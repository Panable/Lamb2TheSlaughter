using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.WSA;


/// <summary>
/// Stores all the door orientations, and the doorinfo
/// </summary>
public class Room //Dhan
{
    /// <summary>
    /// All door orientation prefab objects
    /// </summary>
    public List<GameObject> doors;

    /// <summary>
    /// Stores all door orientation prefab objects plus extra stuff
    /// </summary>
    public List<Door> doorInfo = new List<Door>();
    char roomLetter;

    //Initialize door orientations and the room letter
    public Room(string folderPath)
    {
        doors = Resources.LoadAll<GameObject>(folderPath).ToList();
        roomLetter = doors[0].name.ToCharArray()[0];
        GenerateDoorInfo();

    }

    //used to replace a wall with a door in generation
    public Transform GetDoorPrefab()
    {
        return ProceduralManager.doorPrefabs[roomLetter];
    }

    //populates doorInfo list
    public void GenerateDoorInfo()
    {
        foreach (GameObject door in doors)
        {
            Door tempDoor = new Door(door);
            doorInfo.Add(tempDoor);
        }
    }

    //Ran at the end of the generation to remove the temporary instantiated prefabs
    public void DestroyAll()
    {
        foreach (Door door in doorInfo)
        {
            GameObject.Destroy(door.instantiatedPrefab);
        }
    }
}
