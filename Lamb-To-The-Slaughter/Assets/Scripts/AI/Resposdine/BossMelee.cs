using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMelee : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private bool meleeHit = false;
    private GameObject player;
    #endregion

    //Find the player
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Damage the player
    void Update()
    {
        if (meleeHit)
        {
            player.GetComponent<Health>().TakeDamage(15f);
            meleeHit = false;
        }
    }

    //Control Player Damage
    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "Player")
        {
            meleeHit = true;
        }
    }
}
