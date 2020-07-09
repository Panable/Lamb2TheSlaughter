using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    float healFactor = 150f;

    private void OnTriggerStay(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().currentHealth = healFactor;
            other.GetComponent<PlayerHealth>().OnMedPackUpdate();
        }
    }
}
