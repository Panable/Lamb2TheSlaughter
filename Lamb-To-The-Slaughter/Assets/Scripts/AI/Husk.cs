using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Husk : MonoBehaviour //Lachlan
{
    //For Animations
    public Animator anim;
    public ParticleSystem Injured;

    //Components for the enemy + know where player is
    public Transform player;
    public NavMeshAgent huskAgent;
    private Rigidbody huskRB;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        huskAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        huskRB = gameObject.GetComponent<Rigidbody>();
        anim.SetBool("isMoving", false);
    }

    private void Update()
    {
        //Constantly looks and moves toward player
        transform.LookAt(player.position);
        skulkMoving();
    }

    //Update animation depending on if its moving
    void skulkMoving()
    {
        huskAgent.destination = player.position;

        if (huskAgent.isStopped == false)
        {
            anim.SetBool("isMoving", true);
        }
        if (huskAgent.isStopped == true)
        {
            anim.SetBool("isMoving", false);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damagePlayer();
            anim.SetBool("isMoving", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isAttacking", true);
            Invoke("recover", 1.4f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isAttacking", false);
            Invoke("recover", 1.4f);
        }
    }

    //Deals damage to the player and pushes back the enemy (like a tiny tiny bit)
    void damagePlayer()
    {
            player.GetComponent<Health>().TakeDamage(7f);
    }

    //When the enemy is injured spawn particles
    void hurt()
    {
        Instantiate(Injured, transform.localPosition, transform.localRotation);
    }

    //For recover animation
    void recover()
    {
        anim.SetBool("isMoving", true);
    }
}
