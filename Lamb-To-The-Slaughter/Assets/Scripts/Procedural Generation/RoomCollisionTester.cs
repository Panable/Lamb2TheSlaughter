using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollisionTester : MonoBehaviour
{

    public bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        isColliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }
}
