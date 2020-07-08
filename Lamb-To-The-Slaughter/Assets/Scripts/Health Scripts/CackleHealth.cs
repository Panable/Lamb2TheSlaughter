using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CackleHealth : Health //Dhan
{
    public ParticleSystem hurtParticles;
    public SphereCollider hitBox;
    public Transform particleLocation;

    //Audio
    public AudioClip[] cackleCry;
    public AudioSource sourceCry;
    public AudioClip[] laughs;
    public AudioSource sourceLaugh;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        sourceCry.loop = false;
        sourceLaugh.clip = laughs[Random.Range(0, laughs.Length)];
        sourceLaugh.loop = true;
        sourceLaugh.Play();
    }

    public override void TakeDamage(float amount)
    {
        //Playing Audio when hit
        sourceCry.clip = cackleCry[Random.Range(0, cackleCry.Length)];
        sourceCry.Play();

        //we are taking dmg here
        base.TakeDamage(amount);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, particleLocation.position, transform.rotation);
    }
}