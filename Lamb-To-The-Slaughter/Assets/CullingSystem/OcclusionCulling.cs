using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCulling : MonoBehaviour
{
    GameObject[] sceneObjects;
    [SerializeField] List<GameObject> roomObjects;
    List<Transform> roomChildren;
    List<Transform> roomObjectTransforms;
    [SerializeField] List<GameObject> culledObjects;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("FindRooms", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject cull in culledObjects)
        {
            CullObject(cull);
        }
    }

    void FindRooms()
    {
        sceneObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject cull in sceneObjects)
        {
            if (cull.activeInHierarchy && cull.GetComponent<RoomManager>())
            {
                roomObjects.Add(cull);
            }
        }

        for (int i = 0; i < roomObjects.Count; i++)
        {
            foreach(Transform child in roomObjects[i].transform)
            {
                if (child.CompareTag("GPSGraphic"))
                {
                    child.gameObject.layer = LayerMask.NameToLayer("GPS");
                }
                if (child.CompareTag("roomcollider") && !child.GetComponent<RoomCollider>())
                {
                    child.gameObject.AddComponent<PlayerCheck>();
                }
                if (child.GetComponent<Renderer>() && !CompareTag("Enemy") && !CompareTag("GPSGraphic"))
                {
                    culledObjects.Add(child.gameObject);
                }
            }
        }

        foreach (GameObject cull in culledObjects)
        {
            cull.AddComponent<RenderFunctions>();
        }
    }

    void CullObject (GameObject go)
    {
        if (go.GetComponent<RenderFunctions>())
        {
            Destroy(this);
        }

    }
}
