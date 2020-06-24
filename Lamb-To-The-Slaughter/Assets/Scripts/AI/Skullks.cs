using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skullks : MonoBehaviour //AS IF it was Lachlan
{
    //For Animations
    Animator anim;
    public GameObject player;
    NavMeshAgent skulkAgent;

    //Attack Properties
    [Header("Projectile Properties")]
    [SerializeField]
    private float targetDist;
    public Rigidbody projectile;
    public Transform projectileAnchor;
    public float projectileForce;
    public float shootDelay;
    float shootTimer;
    bool canShoot;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        skulkAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
    }

    void Update()
    {
        skulkAgent.SetDestination(player.transform.position);

        targetDist = Vector3.Distance(transform.position, player.transform.position);
        if (targetDist < 15)
        {
            SkulkAttack();
        }

        if (canShoot)
        {
            Rigidbody projectileInstance = Instantiate(projectile, projectileAnchor.position, projectileAnchor.localRotation);
            projectileInstance.velocity = projectileForce * projectileAnchor.forward;
            canShoot = false;
        }
    }

    void SkulkAttack()
    {
        shootDelay = shootTimer;
        shootTimer -= Time.deltaTime;

        if (shootTimer < 0)
        {
            canShoot = true;
        }
    }
}
