using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skullks : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private GameObject player;
    private NavMeshAgent skulkAgent;
    private float targetDist;
    private float projectileForce = 50f;
    private bool canShoot;
    private Vector3 aimOffset = new Vector3(0, 3, 0);
    private bool isShooting = false;

    public GameObject fireParticles;
    public Rigidbody projectile;
    public Transform projectileAnchor;
    #endregion

    //Initialisation
    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        skulkAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //Look at player & Set projectile anchor rotation
    private void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
        projectileAnchor.LookAt(player.transform.position + aimOffset);
    }

    //Move skulk & find distance from player
    void Update()
    {
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

    //Regulate shooting
    void SkulkAttack()
    {
        if (isShooting) return;

        if (canShoot)
        {
            StartCoroutine(ProjectileAttack(0.5f));
        }
    }

    //Projectile Attack
    IEnumerator ProjectileAttack (float delay)
    {
        isShooting = true;
        yield return new WaitForSeconds(delay);
        Rigidbody projectileInstance = Instantiate(projectile, projectileAnchor.position, projectileAnchor.localRotation);
        projectileInstance.velocity = projectileForce * projectileAnchor.forward;
        //Instantiate(fireParticles, projectileAnchor.position, projectileAnchor.localRotation);
        isShooting = false;
    }
}
