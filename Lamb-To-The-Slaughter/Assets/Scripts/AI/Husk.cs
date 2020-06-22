using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Husk : MonoBehaviour //Lachlan
{
    //For Animations
    public Animator anim;
    public ParticleSystem Injured;
    private float timer;

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
        timer = 1f;
    }

    private void Update()
    {
        //Constantly looks and moves toward player
        huskMoving();
    }

    //Update animation depending on if its moving
    void huskMoving()
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isAttacking", true);
            Invoke("recover", 2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetBool("isAttacking", false);
            Invoke("recover", 2f);
        }
    }

    //Deals damage to the player and pushes back the enemy (like a tiny tiny bit)
    void damagePlayer()
    {
        anim.SetBool("isAttacking", true);
        anim.SetBool("isMoving", false);
        if (timer <= 0 && !anim.IsInTransition(2))
        {
            player.GetComponent<Health>().TakeDamage(7f);
            timer = 1.1f;
            anim.SetBool("isAttacking", false);
        }
        else timerCount();
    }

    void timerCount()
    {
        timer -= Time.deltaTime;
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
