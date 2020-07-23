using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour //Ansaar
{
    public bool playerPresent;

    //Set bool when player has entered the room
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPresent = true;
        }
    }

    //Set bool when player has left the room 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPresent = false;
        }
    }
}
