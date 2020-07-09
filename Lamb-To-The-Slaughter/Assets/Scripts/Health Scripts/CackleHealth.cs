using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CackleHealth : Health //Ansaar(Particles) & Lachlan(Audio)
{
    #region Variables
    public ParticleSystem hurtParticles;
    public SphereCollider hitBox;
    public Transform particleLocation;

    public AudioClip[] cackleCry;
    public AudioSource sourceCry;
    public AudioClip[] laughs;
    public AudioSource sourceLaugh;
    #endregion

    //Initialisation
    protected override void Start()
    {
        base.Start();
        sourceCry.loop = false;
        sourceLaugh.clip = laughs[Random.Range(0, laughs.Length)];
        sourceLaugh.loop = true;
        sourceLaugh.Play();
    }

    //Properties when taking damage
    public override void TakeDamage(float amount)
    {
        sourceCry.clip = cackleCry[Random.Range(0, cackleCry.Length)];
        sourceCry.Play();

        base.TakeDamage(amount);

        Instantiate(hurtParticles, particleLocation.position, transform.rotation);
    }
}