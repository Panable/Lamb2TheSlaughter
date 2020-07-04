using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResHealth : Health
{
    //Health Properties
    public float health;
    public bool isDead = false;
    public ParticleSystem hurtParticles;
    Vector3 particleLocation;
    public CapsuleCollider mainCollider;
    public GameObject canvas;
    PauseMenu bossUI;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        bossUI = canvas.GetComponent<PauseMenu>();
        bossUI.bossIsActive = true;
        bossUI.bossName.SetText("Resposdine: ");
    }

    // Update is called once per frame
    void Update()
    {
        particleLocation = transform.TransformPoint(mainCollider.center);

        health = base.currentHealth;
        bossUI.bossHealth.SetText(health.ToString() + "%");

    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }
}
