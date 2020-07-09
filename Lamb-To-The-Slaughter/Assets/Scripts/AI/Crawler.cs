using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Animator anim;
    private Transform player;
    private NavMeshAgent agent;
    #endregion

    //Initialisation
    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //Look & move towards the player
    private void FixedUpdate()
    {
        //Constantly looks at player
        transform.LookAt(player.transform.position);
        agent.destination = player.position;
    }

    //Damage the player
    private void OnCollisionEnter(Collision collision)
    {
        //When hits the player
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(4f);
        }
    }
}
