using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MaggHealth : Health //Ansar
{
    public ParticleSystem hurtParticles;
    public CapsuleCollider collider;
    public Transform particleLocation;
    private AudioSource audioSourceM;
    public AudioClip cryM;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        audioSourceM = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        audioSourceM.PlayOneShot(cryM, 20f);
        base.TakeDamage(amount);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, particleLocation.position, transform.localRotation);
    }

}
