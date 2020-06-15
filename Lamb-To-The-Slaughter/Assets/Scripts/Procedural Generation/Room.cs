using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

public class Room
{
    public List<GameObject> doors;
    public List<Door> doorInfo = new List<Door>();
    char roomLetter;

    public Room(string folderPath)
    {
        doors = Resources.LoadAll<GameObject>(folderPath).ToList();
        roomLetter = doors[0].name.ToCharArray()[0];
        GenerateDoorInfo();

    }

    public Transform GetDoorPrefab()
    {
        return ProceduralManager.doorPrefabs[roomLetter];
    }

    public void GenerateDoorInfo()
    {
        foreach (GameObject door in doors)
        {
            Door tempDoor = new Door(door);
            doorInfo.Add(tempDoor);
        }
    }

    public void DestroyAll()
    {
        foreach (Door door in doorInfo)
        {
            GameObject.Destroy(door.instantiatedPrefab);
        }
    }
}
