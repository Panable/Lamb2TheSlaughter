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
    public GameObject playerG;
    public NavMeshAgent skullkAgent;
    private Rigidbody skullkRB;
    public BoxCollider boxCol;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        skullkAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerG = GameObject.FindGameObjectWithTag("Player");
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerG.GetComponent<Health>().TakeDamage(10f);
            player.GetComponent<Health>().TakeDamage(10f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        player.GetComponent<Health>().TakeDamage(1f);
    }
}
