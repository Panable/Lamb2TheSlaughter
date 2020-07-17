using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Transform player;
    private CameraShake cameraShake;
    private GameObject cam;
    private bool hitGround;
    private string bombTag;
    private bool done = false;

    //ExplosiveBombs
    public float explosiveForce;
    public float explosiveRadius;
    public GameObject explosiveBombIcon;
    public GameObject explosiveParticleSystem;

    //GasBombs
    public float gasDamage;
    public float gasRadius;
    public bool bombActive;
    public GameObject gasParticleSystem;
    public GameObject gasBombIcon;

    //GravityBombs
    public float gravityForce;
    public float gravityRadius;
    public GameObject gravityParticleSystem;
    public GameObject gravityBombIcon;

    //TeleportBombs
    public Transform tpLocation;
    public GameObject tpParticleSystem;
    public GameObject teleportBombIcon;
    public static GameObject teleport;
    public static bool killTeleportBomb = false;
    #endregion

    //Initialise
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        cameraShake = cam.GetComponent<CameraShake>();
        bombActive = false;
        bombTag = gameObject.tag;
    }

    //Check Bomb Type
    void Update()
    {
        if (!hitGround) return;
        switch (bombTag)
        {
            case "Bomb_Gravity":
                GravityBomb();
                break;
            case "Bomb_Gas":
                GasBomb();
                break;
            case "Bomb_Explosive":
                ExplosiveBomb();
                break;
            case "Bomb_Teleport":
                TeleportBomb();
                break;
        }

        if (killTeleportBomb)
        {
            DeactivateTlpBomb();
            killTeleportBomb = false;
        }
    }

    //Check Collision
    private void OnCollisionEnter(Collision collision)
    {
        hitGround = true;
    }

    //Explosive Bomb Function
    private void ExplosiveBomb()
    {
        if (done) return;
        done = true;
        MeshRenderer rend = GetComponent<MeshRenderer>();
        explosiveParticleSystem.SetActive(true);
        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, explosiveRadius);
        foreach (Collider hit in nearbyEnemy)
        {
            Rigidbody forceRb = hit.GetComponent<Rigidbody>();

            if (hit.tag == "Enemy")
                hit.GetComponent<Health>().TakeDamage(20);

            rend.enabled = false;
            Invoke("DestroyBomb", 1f);
        }
    }

    //Gas Bomb Function
    private void GasBomb()
    {
        bombActive = true;
        gasParticleSystem.SetActive(true);
        Invoke("DeactivateGasBomb", 5f);

        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, gasRadius);

        foreach (Collider hit in nearbyEnemy)
        {
            Transform enemyTransform = hit.GetComponent<Transform>();

            if (enemyTransform.gameObject.tag == "Enemy")
            {
                enemyTransform.GetComponent<Health>().TakeDamage(gasDamage * Time.deltaTime);
            }
        }
    }

    //Gravity Bomb Function
    private void GravityBomb()
    {
        gravityParticleSystem.SetActive(true);
        Invoke("DeactivateGravBomb", 5f);

        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, gravityRadius);
        foreach (Collider hit in nearbyEnemy)
        {
            Transform enemy = hit.GetComponent<Transform>();
            Rigidbody bombRb = transform.GetComponent<Rigidbody>();
            Collider enemyCol = hit.GetComponent<Collider>();
            if (enemy != null && enemy.tag == "Enemy")
            {
                enemy.transform.position = Vector3.Lerp(enemy.transform.position, gameObject.transform.position, Time.deltaTime * gravityForce);
            }
        }
    }

    //Teleport Bomb Function
    private void TeleportBomb()
    {
        teleport = gameObject;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        tpParticleSystem.SetActive(true);
        bombActive = true;
        player.GetComponent<PlayerMovementCC>().TeleportFunction(gameObject);
    }

    //Destroy Bomb
    private void DestroyBomb()
    {
        Destroy(gameObject);
    }

    //Remove Functionality (GravBomb)
    private void DeactivateGravBomb()
    {
        Destroy(gravityParticleSystem);
        Destroy(this);
    }

    //Remove Functionality (GasBomb)
    private void DeactivateGasBomb()
    {
        Destroy(gasParticleSystem);
        Destroy(this);
    }

    void DeactivateTlpBomb()
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Destroy(this);
    }
}