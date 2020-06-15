using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Stores temporary instantiated objects, collider information, room manager information, and position of door orientations
/// </summary>
public class Door
{
    GameObject prefab;
    public GameObject instantiatedPrefab;
    public BoxCollider collider;
    public Vector3 position;
    public Quaternion rotation;
    public RoomManager roomManager;
    public Door(GameObject prefab)
    {
        this.prefab = prefab;
        instantiatedPrefab = GameObject.Instantiate<GameObject>(prefab);
        collider = instantiatedPrefab.GetComponent<RoomManager>().roomCollider;
        if (collider == null)
            Debug.LogError(prefab.name);
        roomManager = instantiatedPrefab.GetComponent<RoomManager>();
        //instantiatedPrefab.SetActive(false);
    }

    public GameObject activateObj(Vector3 pos, Quaternion rot)
    {
        instantiatedPrefab.SetActive(true);
        instantiatedPrefab.transform.position = pos;
        collider = instantiatedPrefab.GetComponent<RoomManager>().InstantiateCollider();
        return instantiatedPrefab;
    }

    public void deactivateObj()
    {
       //instantiatedPrefab.SetActive(false);
    }

}
