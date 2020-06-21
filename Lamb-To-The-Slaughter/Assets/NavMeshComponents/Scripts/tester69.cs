using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester69 : MonoBehaviour
{

    public GameObject currentDoor;
    public Transform spawnDoor;
    public bool fix1;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool fix2;
    // Update is called once per frame
    void Update()
    {
        if (fix1)
        {
            currentDoor.transform.position = spawnDoor.position;
            
        }
        if (fix2)
            currentDoor.transform.forward = spawnDoor.up;
    }
}
