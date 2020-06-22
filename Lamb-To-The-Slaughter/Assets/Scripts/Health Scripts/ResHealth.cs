using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResHealth : Health
{
    public float health;
    public bool isDead = false;
    public ParticleSystem hurtParticles;
    Vector3 particleLocation;
    public CapsuleCollider mainCollider;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        particleLocation = transform.TransformPoint(mainCollider.center);

        health = base.currentHealth;

        if (health < 1)
        {
            isDead = true;
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }
}
