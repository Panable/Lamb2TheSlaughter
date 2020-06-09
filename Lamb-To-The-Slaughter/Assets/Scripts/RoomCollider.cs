using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollider : MonoBehaviour
{

    public bool isColliding;

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
        if (other.transform.tag == "roomcollider")
        {
            //Debug.Log("is Colliding");
            isColliding = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "roomcollider")
        {
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "roomcollider")
        {
            isColliding = false;
        }
    }

}
