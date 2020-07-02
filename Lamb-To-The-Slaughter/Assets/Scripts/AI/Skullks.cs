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
    public GameObject fireParticles;
    Vector3 aimOffset = new Vector3(0, 3, 0);

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
        projectileAnchor.LookAt(player.transform.position + aimOffset);
    }

    void Update()
    {
        Debug.Log("Can skulks shoot?: " + canShoot);

        skulkAgent.SetDestination(player.transform.position);

        targetDist = Vector3.Distance(transform.position, player.transform.position);
        if (targetDist < 15)
        {
            canShoot = true;
            SkulkAttack();
        }
        else
        {
            canShoot = false;
        }
    }

    void SkulkAttack()
    {
        if (isShooting) return;

        if (canShoot)
        {
            StartCoroutine(ProjectileAttack(0.5f));
        }
    }

    bool isShooting = false;

    IEnumerator ProjectileAttack (float delay)
    {
        isShooting = true;
        yield return new WaitForSeconds(delay);
        Rigidbody projectileInstance = Instantiate(projectile, projectileAnchor.position, projectileAnchor.localRotation);
        projectileInstance.velocity = projectileForce * projectileAnchor.forward;
        Instantiate(fireParticles, projectileAnchor.position, projectileAnchor.localRotation);
        isShooting = false;
    }
}
