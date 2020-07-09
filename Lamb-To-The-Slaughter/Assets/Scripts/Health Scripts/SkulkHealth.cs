using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulkHealth : Health //Ansaar(Particles) & Lachlan(Audio)
{
    #region Variables
    private Vector3 particleLocation;
    private AudioSource crySource;

    public ParticleSystem hurtParticles;
    public SphereCollider hitBox;
    public AudioClip skulkCry;
    #endregion

    //Initialisation
    protected override void Start()
    {
        base.Start();
        crySource = GetComponent<AudioSource>();
    }

    //Properties when taking damage
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        crySource.PlayOneShot(skulkCry, 50f);
        particleLocation = transform.TransformPoint(hitBox.center);
        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }
}

