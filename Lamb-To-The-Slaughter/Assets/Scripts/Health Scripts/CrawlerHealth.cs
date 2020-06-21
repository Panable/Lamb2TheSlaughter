using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerHealth : Health
{
    public ParticleSystem hurtParticles;
    private AudioSource audioSourceC;
    public AudioClip cryC;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        audioSourceC = GetComponent<AudioSource>();
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        base.TakeDamage(amount);
        audioSourceC.PlayOneShot(cryC, 10);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, transform.localPosition, transform.rotation);
    }

}