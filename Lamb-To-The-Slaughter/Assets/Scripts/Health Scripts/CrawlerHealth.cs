using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CrawlerHealth : Health //Ansaar(Particles) & Lachlan(Audio)
{
    #region Variables
    public ParticleSystem hurtParticles;
    public Vector3 particleLocation;
    public CapsuleCollider collider;

    public AudioSource audioSourceC;
    public AudioSource sourceLaugh;
    public AudioClip[] laughs;
    public AudioClip[] cryC;
    #endregion


    //Initialisation
    protected override void Start()
    {
        base.Start();
        collider = GetComponent<CapsuleCollider>();
        sourceLaugh.clip = laughs[Random.Range(0, laughs.Length)];
        sourceLaugh.loop = true;
        sourceLaugh.Play();
    }

    //Properties when taking damage
    public override void TakeDamage(float amount)
    {
        audioSourceC.clip = cryC[Random.Range(0, cryC.Length)];
        audioSourceC.loop = false;
        audioSourceC.Play();
        base.TakeDamage(amount);
        Instantiate(hurtParticles, particleLocation, transform.rotation);
    }

    //Update the location of the particles
    private void Update()
    {
        particleLocation = transform.TransformPoint(collider.center);
    }

}