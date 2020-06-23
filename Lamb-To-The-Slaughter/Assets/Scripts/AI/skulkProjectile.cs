using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skulkProjectile : MonoBehaviour
{
    public GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerHealth>().TakeDamage(43f);
            Destroy(this.gameObject);
        }
    }
}
