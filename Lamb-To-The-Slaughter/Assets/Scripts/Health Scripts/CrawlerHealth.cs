using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CrawlerHealth : Health //Dhan
{
    public ParticleSystem hurtParticles;
    Vector3 particleLocation;
    CapsuleCollider collider;

    //Audio
    public AudioSource audioSourceC;
    public AudioSource sourceLaugh;
    public AudioClip[] laughs;
    public AudioClip[] cryC;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        collider = GetComponent<CapsuleCollider>();
        sourceLaugh.clip = laughs[Random.Range(0, laughs.Length)];
        sourceLaugh.loop = true;
        sourceLaugh.Play();
    }

    public override void TakeDamage(float amount)
    {
        //Playing audio when Crawler is hit/damaged
        audioSourceC.clip = cryC[Random.Range(0, cryC.Length)];
        audioSourceC.loop = false;
        audioSourceC.Play();
        //we are taking dmg here and spawning particles
        base.TakeDamage(amount);
        Instantiate(hurtParticles, particleLocation, transform.rotation);
    }

    private void Update()
    {
        particleLocation = transform.TransformPoint(collider.center);
    }

}