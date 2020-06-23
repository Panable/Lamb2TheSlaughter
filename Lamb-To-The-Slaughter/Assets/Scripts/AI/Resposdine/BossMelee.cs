using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMelee : MonoBehaviour
{
    bool meleeHit = false;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (meleeHit)
        {
            player.GetComponent<Health>().TakeDamage(15f);
            meleeHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player")
        {
            meleeHit = true;
        }
    }
}
