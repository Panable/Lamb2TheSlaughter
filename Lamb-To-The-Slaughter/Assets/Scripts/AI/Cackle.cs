using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cackle : MonoBehaviour //Lachlan
{
    //For Animations
    Animator anim;

    //Components for the enemy + know where player is
    public Transform player;
    public NavMeshAgent agent;


    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.isStopped = true;
    }

    private void FixedUpdate()
    {
        //Constantly looks at player
        transform.LookAt(player.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        //When hits the player
        if (collision.gameObject.tag == "Player")
        {
            damagePlayer();
        }
    }

    void Update()
    {
        //check if what way the gameObject is rotating.
        //you need a float that updates between 0 & 1.
        //0 = turning left & 1 = turning right.

        //Collision that takes and inflicts damage must be a sphere collider on the head
    }

    //Deals damage to the player and pushes back the enemy (like a tiny tiny bit)
    void damagePlayer()
    {
        player.GetComponent<Health>().TakeDamage(5f);
    }
}
