using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maggots : MonoBehaviour //Lachlan
{
    //Maggot's Gameobject components
    private NavMeshAgent maggotAgent;
    private Rigidbody maggotRB;
    public Transform player;

    //Particles to spawn when damaged
    public ParticleSystem Injured;

    //Variables forwonder
    public float wanderRadius = 50;
    public float wanderTimer = 3.7f;
    private float timer;
    public float bounceThreshold;

    //Variables for bouncyboi
    float bounceHeight;
    public GameObject mainBone;

    //CollisionFix
    CapsuleCollider col;

    //Finds The Componenets Neccessary for the enemy to move and finds the target to avoid.
    void OnEnable()
    {
        col = gameObject.GetComponent<CapsuleCollider>();
        maggotAgent = GetComponent<NavMeshAgent>();
        maggotRB = GetComponent<Rigidbody>();
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnCollisionStay(Collision collision)
    {
        Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
        maggotAgent.SetDestination(newPos);
        timer = 0;

        if (collision.gameObject.tag == "Player")
        {
            hurtPlayer();
        }
    }

    void hurtPlayer()
    {
        player.GetComponent<Health>().TakeDamage(6f);
    }

    //Function spawns particles that indicate the enemy has been hit
    void Enemyishurt()
    {
        Instantiate(Injured, transform.localPosition, transform.localRotation);
    }

    //Update the timer to change direction when it runs out
    void Update()
    {
        bouncyBoi();

        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            maggotAgent.SetDestination(newPos);
            timer = 0;
        }

        if (maggotAgent.isStopped == true)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            maggotAgent.SetDestination(newPos);
            timer = 0;
        }
    }

    //Make Maggot bounce
    public void bouncyBoi()
    {
        Vector3 originalCentre = new Vector3(col.center.x, 1.6f, col.center.z);
        Vector3 bounceCentre = new Vector3(col.center.x, 5f, col.center.z);

        RaycastHit hit;
        Ray downray = new Ray(mainBone.transform.position, -Vector3.up);

        if (Physics.Raycast(downray, out hit))
        {
            bounceHeight = hit.distance;
        }

        if (bounceHeight > bounceThreshold)
        {
            col.center = bounceCentre;
            maggotAgent.speed = 4.5f;
        }
        else
        {
            col.center = originalCentre;
            maggotAgent.speed = 0f;
        }
    }

    //Getting random location
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
