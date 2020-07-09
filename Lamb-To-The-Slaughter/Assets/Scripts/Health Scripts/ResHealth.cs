using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResHealth : Health //Ansaar
{
    #region Variables
    [SerializeField]
    private Vector3 particleLocation;
    private PauseMenu bossUI;
    private GameObject deathParticles;

    public float health;
    public bool isDead = false;
    public ParticleSystem hurtParticles;
    public CapsuleCollider mainCollider;
    public GameObject canvas;
    #endregion

    //Properties for when killed
    public override void OnDeath()
    {
        deathParticles.SetActive(true);
        Destroy(gameObject);
    }

    //Initialisation
    void Start()
    {
        base.Start();
        deathParticles = gameObject.transform.parent.Find("DeathParticles").gameObject;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        bossUI = canvas.GetComponent<PauseMenu>();
        bossUI.bossIsActive = true;
        bossUI.bossName.SetText("Resposdine: ");
    }

    //GUI, Particle Location Control & Death Check
    void Update()
    {
        particleLocation = transform.TransformPoint(mainCollider.center);

        health = base.currentHealth;
        bossUI.bossHealth.SetText(health.ToString() + "%");

        if (health < 1)
        {
            OnDeath();
        }
    }

    //Properties when taking damage
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }
}
