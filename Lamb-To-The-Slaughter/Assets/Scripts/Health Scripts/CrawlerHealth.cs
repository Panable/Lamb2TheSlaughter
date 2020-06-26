using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerHealth : Health //Dhan
{
    public ParticleSystem hurtParticles;
    private AudioSource audioSourceC;
    public AudioClip cryC;
    Vector3 particleLocation;
    CapsuleCollider collider;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        audioSourceC = GetComponent<AudioSource>();
        collider = GetComponent<CapsuleCollider>();
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        audioSourceC.PlayOneShot(cryC, 10);
        base.TakeDamage(amount);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, particleLocation, transform.rotation);
    }

    private void Update()
    {
        particleLocation = transform.TransformPoint(collider.center);
    }

}