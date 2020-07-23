﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResProjectile : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Rigidbody rb;
    private SphereCollider sc;
    private GameObject resposdine;
    private Renderer rend;

    public ResHealth rHealth;
    public GameObject flares;
    public Vector3 hitScale;
    float fade = 0;
    #endregion

    //Initialisation
    void Awake()
    {
        resposdine = GameObject.FindGameObjectWithTag("Enemy");
        rHealth = resposdine.GetComponent<ResHealth>();
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        rend = GetComponent<Renderer>();
    }

    //Control Projectile color
    void Update()
    {
        if (rHealth == null)
        {
            sc.enabled = false;
            rend.material.SetFloat("Fade", fade);
            fade += 0.05f;
            if (fade > 0.95f)
            {
                Destroy(gameObject);
            }
        }
    }

    //Regulate collision effect
    private void OnCollisionEnter(Collision other)
    {
        transform.localScale = hitScale;
        Destroy(flares);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        sc.isTrigger = true;
        sc.radius = 0.4f;
    }

    //Damage Player
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(5f);
        }
    }
}
