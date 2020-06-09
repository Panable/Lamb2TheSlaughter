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

    private void FixedUpdate()
    {
        //Constantly looks and moves toward player
        transform.LookAt(player.position);
        huskAgent.destination = player.position;
        skulkMoving();
    }

    //Update animation depending on if its moving
    void skulkMoving()
    {
        if (huskAgent.isStopped == false)
        {
            anim.SetBool("isMoving", true);
        }
        if (huskAgent.isStopped == true)
        {
            anim.SetBool("isMoving", false);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        //When hits the player damage them and take a sec to chill out cause it's a strong boi
        if (collision.gameObject.tag == "Player")
        {
            damagePlayer();
            Invoke("recover", 1.2f);
        }

        // When Hit By bullet
        if (collision.gameObject.tag == "PBullet")
        {
            hurt();
            anim.SetBool("isMoving", false);
            Invoke("recover", 1f);
        }
    }

    //Deals damage to the player and pushes back the enemy (like a tiny tiny bit)
    void damagePlayer()
    {
        huskAgent.isStopped = true;
        //Add Damage To Player Here
        player.GetComponent<Health>().TakeDamage(15f);
        huskAgent.isStopped = false;
    }

    //When the enemy is injured spawn particles, temp stop from moving, resume attack
    void hurt()
    {
        Instantiate(Injured, transform.localPosition, transform.localRotation);
        huskAgent.isStopped = true;
        huskAgent.isStopped = false;
    }

    //For recover animation
    void recover()
    {
        anim.SetBool("isMoving", true);
    }
}
