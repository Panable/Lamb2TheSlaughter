using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] bool forceGeneration;
    private List<Room> roomPrefabsToTryGenerate;
    bool hasGenerated = false;


    //Generate every position of a current room
    //switch room

    public void Awake()
    {

    }

    public bool GenerateRoom(Transform spawnDoor, RoomManager rm)
    {
        Debug.Log("Generating..." + rm.transform.name);
        Debug.Log(roomPrefabsToTryGenerate.Count);
        Debug.Log(spawnDoor.name);
        Vector3 direction;
        float distance;

        roomPrefabsToTryGenerate.Shuffle();
        while (!hasGenerated)
        {
            if (roomPrefabsToTryGenerate.Count == 0)
            {
                Debug.Log("genfail");
                return false;
            }
            Room currentRoom = roomPrefabsToTryGenerate[0];
            List<Door> possibleDoorSpots = new List<Door>();
            currentRoom.doorInfo.ForEach(i => possibleDoorSpots.Add(i));
            possibleDoorSpots.Shuffle();

            GameObject currentDoor = possibleDoorSpots[0].activateObj(spawnDoor.position, Quaternion.identity);
            currentDoor.transform.forward = spawnDoor.up;


            bool colliding = true;
            while (colliding)
            {
                if (possibleDoorSpots.Count == 0)
                {
                    roomPrefabsToTryGenerate.RemoveAt(0);
                    break;
                }

                bool collidingWithOtherThings = false;

                foreach (RoomManager managers in ProceduralManager.roomsGenerated)
                {
                    if (Physics.ComputePenetration(managers.roomCollider, ExtensionMethods.ColliderToWorldPoint(managers.roomCollider), managers.transform.rotation,
                possibleDoorSpots[0].collider, ExtensionMethods.ColliderToWorldPoint(possibleDoorSpots[0].collider), currentDoor.transform.rotation, out direction, out distance))
                    {
                        collidingWithOtherThings = true;
                        break;
                    }
                }

                if (collidingWithOtherThings)
                {
                    Debug.Log("cuz coll");
                    possibleDoorSpots[0].roomManager.roomColliderScript.isColliding = false;
                    possibleDoorSpots.RemoveAt(0);
                    if (possibleDoorSpots.Count == 0)
                    {
                        roomPrefabsToTryGenerate.RemoveAt(0);
                        break;
                    }
                    currentDoor = possibleDoorSpots[0].activateObj(spawnDoor.position, Quaternion.identity);
                    currentDoor.transform.forward = spawnDoor.up;
                    continue;
                }
                else
                {
                    currentDoor = Instantiate(currentDoor);
                    currentDoor.transform.name = ProceduralManager.numberOfRoomsGenerated.ToString() + " from " + spawnDoor.parent.transform.name; 
                    if (currentDoor != null)
                    {
                        rm.roomsGenerated.Add(currentDoor.gameObject);


                        ProceduralManager.roomsGenerated.Add(currentDoor.GetComponent<RoomManager>());
                        ProceduralManager.roomsToGenerate.Add(currentDoor.GetComponent<RoomManager>());
                    }
                    Quaternion rot = Quaternion.LookRotation(Vector3.up, currentDoor.transform.forward);
                    if (rm.spawnRoom)
                    {
                        rot = Quaternion.LookRotation(Vector3.up, currentDoor.transform.right);
                    }
                    Transform doorReplace = Instantiate<Transform>(currentRoom.GetDoorPrefab(), spawnDoor.position, rot, spawnDoor.transform.parent);
                    Destroy(spawnDoor.gameObject);
                    //doorReplace.up = spawnDoor.up;
                    return true;
                }
            }

        }
        return false;

    }


    void PositionRoomCorrectly(GameObject room, GameObject roomDoor, Transform spawnRoom)
    {

    }

    public void InstantiateWall()
    {
        Debug.Log("we failed");
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
        Debug.Log("procedural managers room size is: " + ProceduralManager.roomPrefabs.Count);
        Debug.Log("room generator's room prefab size is " + roomPrefabsToTryGenerate.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (forceGeneration)
        {
            //InstantiateRoomPrefabs();
            Transform spawnRoom = GetComponentInParent<Transform>();
            RoomManager rm = GetComponentInParent<RoomManager>();
            GenerateRoom(spawnRoom, rm);

        }
    }
}
