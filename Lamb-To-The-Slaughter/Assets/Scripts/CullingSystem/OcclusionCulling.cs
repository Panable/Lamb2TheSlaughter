using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCulling : MonoBehaviour //Ansaar
{
    #region Variables
    GameObject[] sceneObjects;
    [SerializeField] List<GameObject> roomObjects;
    List<Transform> roomChildren;
    List<Transform> roomObjectTransforms;
    [SerializeField] List<GameObject> culledObjects;
    #endregion

    //Find cullable objects after player has spawned
    void Start()
    {
        Invoke("FindRooms", 3f);
    }

    //Cull cullable objects
    void Update()
    {
        foreach(GameObject cull in culledObjects)
        {
            CullObject(cull);
        }
    }

    //Find every gameobject that needs to be culled
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

    //If the script is assigned, the operation is done hence, destroy this.
    void CullObject (GameObject go)
    {
        if (go.GetComponent<RenderFunctions>())
        {
            Destroy(this);
        }

    }
}
