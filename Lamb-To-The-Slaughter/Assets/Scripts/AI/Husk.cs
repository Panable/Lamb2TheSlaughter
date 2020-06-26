using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Husk : MonoBehaviour //Lachlan
{
    //For Animations
    public Animator anim;
    public ParticleSystem Injured;
    bool inTrigger;

    //Components for the enemy + know where player is
    public Transform player;
    public NavMeshAgent huskAgent;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        huskAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        huskAgent.isStopped = false;
    }

    private void Update()
    {
        Debug.Log(huskAgent.isStopped);

        huskMoving();
    }

    //Update animation depending on if its moving
    void huskMoving()
    {
        if (huskAgent.isStopped)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);
        }
        else if (!huskAgent.isStopped)
        {
            huskAgent.destination = player.position;
            anim.SetBool("isMoving", true);
            anim.SetBool("isAttacking", false);
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (attacking) return;
        //When hits the player
        if (collision.gameObject.tag == "Player")
        {
            inTrigger = true;
            Debug.Log("Contact");
            StartCoroutine(attack(1f, collision));

            //huskAgent.isStopped = true;
            ///collision.gameObject.GetComponent<Health>().TakeDamage(10f);
            //huskAgent.isStopped = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inTrigger = false;
        }
    }

    bool attacking = false;

    IEnumerator attack(float strikeTime, Collider collision)
    {
        attacking = true;
        huskAgent.isStopped = true;
        yield return new WaitForSeconds(strikeTime);
        if (inTrigger)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(10f);
        }
        huskAgent.isStopped = false;
        attacking = false;
    }
}
