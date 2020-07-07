using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class BombScript : MonoBehaviour //Ansaar
{

    public static GameObject teleport;

    //ExplosiveBombs
    public float explosiveForce;
    public float explosiveRadius;
    public GameObject explosiveBombIcon;
    public GameObject explosiveParticleSystem;

    //GasBombs
    public float gasDamage;
    public float gasRadius;
    bool bombActive;
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
    PlayerMovementCC pmcc;
    public GameObject teleportBombIcon;

    Transform player;
    CameraShake cameraShake;
    GameObject cam;

    bool hitGround;
    string bombTag;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        cameraShake = cam.GetComponent<CameraShake>();
        bombActive = false;
        bombTag = gameObject.tag;
    }

    // Update is called once per frame
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitGround = true;
    }

    bool done = false;

    private void ExplosiveBomb()
    {
        if (done) return;
        done = true;
        MeshRenderer rend = GetComponent<MeshRenderer>();
        explosiveParticleSystem.SetActive(true);
        //explosiveBombIcon.SetActive(true);
        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, explosiveRadius);
        foreach (Collider hit in nearbyEnemy)
        {
            Debug.Log("do dmg");
            Rigidbody forceRb = hit.GetComponent<Rigidbody>();

            //StartCoroutine(cameraShake.Shake(0.25f, 1f));
            //Do damage
            if (hit.tag == "Enemy")
                hit.GetComponent<Health>().TakeDamage(20); 

            rend.enabled = false;
            Invoke("DestroyBomb", 1f);
            //explosiveBombIcon.SetActive(false);
        }
    }

    private void GasBomb()
    {
        bombActive = true;
        gasParticleSystem.SetActive(true);
        Invoke("DeactivateGasBomb", 5f);
        //gasBombIcon.SetActive(true);

        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, gasRadius);

        foreach (Collider hit in nearbyEnemy)
        {
            Transform enemyTransform = hit.GetComponent<Transform>();

            if (enemyTransform.gameObject.tag == "Enemy")
            {
                enemyTransform.GetComponent<Health>().TakeDamage(gasDamage * Time.deltaTime);
            }
        }
        //gasBombIcon.SetActive(false);
    }


    private void GravityBomb()
    {
        gravityParticleSystem.SetActive(true);
        Invoke("DeactivateGravBomb", 5f);
        //gravityBombIcon.SetActive(true);

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

        //gravityBombIcon.SetActive(false);
    }

    private void TeleportBomb()
    {
        if (teleport != null)
            Destroy(teleport);
        teleport = gameObject;
        tpLocation = gameObject.transform;
        //teleportBombIcon.SetActive(true);
        tpParticleSystem.SetActive(true);
        bombActive = true;
        player.GetComponent<PlayerMovementCC>().TeleportFunction(gameObject);
        tpLocation.position = gameObject.transform.position;
        //teleportBombIcon.SetActive(false);
    }

    private void DestroyBomb()
    {
        Destroy(gameObject);
    }

    private void DeactivateGravBomb()
    {
        Destroy(gravityParticleSystem);
        Destroy(this);
    }

    private void DeactivateGasBomb()
    {
        Destroy(gasParticleSystem);
        Destroy(this);
    }
}