using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skullks : MonoBehaviour //Lachlan
{
    //For Animations
    Animator anim;
    public ParticleSystem Injured;

    //Components for the enemy + know where player is
    public Transform player;
    public NavMeshAgent skullkAgent;
    private Rigidbody skullkRB;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        skullkAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        skullkRB = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        skulkMoving();
    }

    //Update animation depending on if its moving
    void skulkMoving()
    {

        skullkAgent.destination = player.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        //When hits the player
        if (collision.gameObject.tag == "Player")
        {
            damagePlayer();
        }
    }

    //Deals damage to the player 
    void damagePlayer()
    {
        skullkAgent.isStopped = true;
        player.GetComponent<Health>().TakeDamage(10f);
        skullkAgent.isStopped = false;
    }

    //When the enemy is injured spawn particles
    void hurt()
    {
        
    }
}
