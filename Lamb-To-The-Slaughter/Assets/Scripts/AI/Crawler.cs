using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler : MonoBehaviour //Lachlan
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
    }

    private void FixedUpdate()
    {
        //Constantly looks at player
        transform.LookAt(player.transform.position);
        agent.destination = player.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //When hits the player
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(4f);
        }
    }
}
