using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HuskHealth : Health //Dhan
{
    public ParticleSystem hurtParticles;
    public BoxCollider collider;
    Vector3 particleLocation;

    //Audio
    public AudioSource audioSource;
    public AudioClip[] huskCry;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
      particleLocation = transform.TransformPoint(collider.center);
    }

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
