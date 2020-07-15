using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Door // Dhan
{
    /// <summary>
    /// Stores temporary instantiated objects, collider information, room manager information, and position of door orientations
    /// </summary>

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
    }

    public GameObject activateObj(Vector3 pos, Quaternion rot)
    {
        instantiatedPrefab.SetActive(true);
        instantiatedPrefab.transform.position = pos;
        collider = instantiatedPrefab.GetComponent<RoomManager>().InstantiateCollider();
        return instantiatedPrefab;
    }
}
