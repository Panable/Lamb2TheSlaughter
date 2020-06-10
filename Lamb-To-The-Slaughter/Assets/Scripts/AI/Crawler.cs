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
    private Rigidbody crawlerRB;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        crawlerRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim.SetBool("isMoving", true);
    }

    private void FixedUpdate()
    {
        //Constantly looks at player
        transform.LookAt(player.transform.position);
        agent.destination = player.position;
    }

    void OnCollisionStay(Collision collision)
    {
        //When hits the player
        if (collision.gameObject.tag == "Player")
        {
            damagePlayer();
        }
    }

    //Deals damage to the player and pushes back the enemy (like a tiny tiny bit)
    void damagePlayer()
    {
        agent.isStopped = true;
        player.GetComponent<Health>().TakeDamage(2f);
        agent.isStopped = false;
    }

    //For recover animation
    void recover()
    {
        anim.SetBool("isMoving", true);
    }
}
