using System.Collections;
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

    public GameObject GenerateRoom(Transform spawnDoor, RoomManager rm)
    {
        Vector3 direction;
        float distance;
        bool colliding;

        roomPrefabsToTryGenerate.Shuffle();
        while (true)
        {
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
                if (possibleDoorSpots.Count == 0) { possibleDoorSpots.RemoveAt(0); break; }

                GameObject currentDoor = possibleDoorSpots[0].activateObj(spawnDoor.position, Quaternion.identity);
                currentDoor.transform.forward = spawnDoor.up;

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
                if (colliding)
                {
                    possibleDoorSpots.RemoveAt(0);
                    if (possibleDoorSpots.Count == 0)
                    {
                        roomPrefabsToTryGenerate.RemoveAt(0);
                        break;
                    }
                }
                else
                {
                    currentDoor = Instantiate(currentDoor);


                    //change parent room's door way to match the room generated
                    Quaternion rot = Quaternion.LookRotation(Vector3.up, currentDoor.transform.forward);
                    if (rm.spawnRoom)
                    {
                        rot = Quaternion.LookRotation(Vector3.up, currentDoor.transform.right);
                    }

                    Transform doorReplace = Instantiate<Transform>(currentRoom.GetDoorPrefab(), spawnDoor.position, rot, spawnDoor.transform.parent);
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
