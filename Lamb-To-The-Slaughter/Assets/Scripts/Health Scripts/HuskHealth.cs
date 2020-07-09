using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HuskHealth : Health //Ansaar(Particles) & Lachlan(Audio)
{
    #region Variables
    public ParticleSystem hurtParticles;
    public BoxCollider collider;
    public Vector3 particleLocation;

    public AudioSource audioSource;
    public AudioClip[] huskCry;
    #endregion

    //Initialisation
    protected override void Start()
    {
        base.Start();
        audioSource.loop = false;
    }

    //Update the location of particles
    void Update()
    {
      particleLocation = transform.TransformPoint(collider.center);
    }

    //Properties when taking damage
    public override void TakeDamage(float amount)
    {
        //Plays Audio when Husk is damaged
        audioSource.clip = huskCry[Random.Range(0, huskCry.Length)];
        audioSource.Play();

        //we are taking dmg here
        base.TakeDamage(amount);

        //after damage is taken, blood particles are spawned.
        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }

}
