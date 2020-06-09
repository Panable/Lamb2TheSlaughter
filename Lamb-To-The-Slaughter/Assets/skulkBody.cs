using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skulkBody : MonoBehaviour  // Lachlan
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void OnColliderEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<Health>().TakeDamage(10f);
        }
    }
}
