using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public List<GameObject> doors;
    public List<Door> doorInfo = new List<Door>();
    string RoomName;
    public Room(string folderPath)
    {
        doors = Resources.LoadAll<GameObject>(folderPath).ToList();
        //GenerateDoorPool();
        GenerateDoorInfo();
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
