using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombScript : MonoBehaviour
{
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
    public CameraShake cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bombActive = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "Bomb_Explosive")
        {
            ExplosiveBomb();
        }

       if (gameObject.tag == "Bomb_Teleport")
        {
            tpLocation = gameObject.transform;
            TeleportBomb();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (gameObject.tag == "Bomb_Gravity")
        {
            GravityBomb();
        }

        if (gameObject.tag == "Bomb_Gas")
        {
            GasBomb();
        }
    }

    private void ExplosiveBomb()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        explosiveParticleSystem.SetActive(true);
        //explosiveBombIcon.SetActive(true);
        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, explosiveRadius);
        foreach (Collider hit in nearbyEnemy)
        {
            Rigidbody forceRb = hit.GetComponent<Rigidbody>();

            StartCoroutine(cameraShake.Shake(0.25f, 6f));
            //Do damage
            rend.enabled = false;
            Invoke("DestroyBomb", 1f);
            //explosiveBombIcon.SetActive(false);
        }
    }

    private void GasBomb()
    {
        bombActive = true;
        gasParticleSystem.SetActive(true);
        //gasBombIcon.SetActive(true);

        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, gasRadius);

        foreach (Collider hit in nearbyEnemy)
        {
            Transform enemyTransform = hit.GetComponent<Transform>();

            if (enemyTransform.gameObject.tag == "Enemy")
            {
                //decrease enemy health over time
            }
        }
        //gasBombIcon.SetActive(false);
    }

    private void GravityBomb()
    {
        gravityParticleSystem.SetActive(true);
        //gravityBombIcon.SetActive(true);

        Collider[] nearbyEnemy = Physics.OverlapSphere(transform.position, gravityRadius);
        foreach (Collider hit in nearbyEnemy)
        {
            Transform enemy = hit.GetComponent<Transform>();
            Rigidbody bombRb = transform.GetComponent<Rigidbody>();
            Collider enemyCol = hit.GetComponent<Collider>();
            if (enemy != null && enemy.tag == "Enemy")
            {
                enemy.transform.position = Vector3.Lerp(enemy.transform.position, gameObject.transform.position, gravityForce);
                bombRb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        //gravityBombIcon.SetActive(false);
    }

    private void TeleportBomb()
    {
        //teleportBombIcon.SetActive(true);
        tpParticleSystem.SetActive(true);
        bombActive = true;
        tpLocation.position = gameObject.transform.position;
        //teleportBombIcon.SetActive(false);
    }

    private void DestroyBomb()
    {
        Destroy(gameObject);
    }
}