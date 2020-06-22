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

    //Attack
    public GameObject projectile;
    private BoxCollider radiusCol;
    public Transform skulhead;
    private bool stop;
    public float timer;


    public Rigidbody projectlePrefab;
    public float projectileSpeed;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        skullkAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerG = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindWithTag("Player").transform;
        skullkRB = gameObject.GetComponent<Rigidbody>();
        radiusCol = GetComponent<BoxCollider>();
    }

    void Update()
    {
        skulkMoving();
    }

    //Update animation depending on if its moving
    void skulkMoving()
    {
        if (stop == false)
        {
            skullkAgent.destination = player.position;
        }
        else if (stop == true)
        {
            skullkAgent.destination = gameObject.transform.position;
        }
    }

    void TickingTimer()
    {
        timer -= Time.deltaTime;
    }

    void skulkAttack()
    {
        if (timer <= 0)
        {
            Rigidbody projectleInstance = Instantiate(projectlePrefab, skulhead.transform.position, skulhead.transform.rotation) as Rigidbody;
            projectleInstance.velocity = projectileSpeed * skulhead.transform.position;
            //Instantiate(projectile, skulhead.transform.position, Quaternion.identity);
            //projectile.GetComponent<Rigidbody>().AddForce(projectileSpeed * transform.localPosition * 100f);
            timer = 2;
        }
        else TickingTimer();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Health>().TakeDamage(10f);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stop = true;
            skulkAttack();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stop = false;
        }
    }
}
