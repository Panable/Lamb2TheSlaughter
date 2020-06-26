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
    //private Rigidbody crawlerRB;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //crawlerRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //anim.SetBool("isMoving", true);
    }

    private void FixedUpdate()
    {
        //Constantly looks at player
        transform.LookAt(player.transform.position);
        agent.destination = player.position;
    }

    void OnTriggerEnter(Collider collision)
    {
        //When hits the player
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(5f);
        }
    }
}
