using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour //Ansaar
{
    float healFactor = 150f;

    //When player stays in the collider, heal the player by the float healfactor.
    private void OnTriggerStay(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().currentHealth = healFactor;
            other.GetComponent<PlayerHealth>().OnMedPackUpdate();
        }
    }
}
