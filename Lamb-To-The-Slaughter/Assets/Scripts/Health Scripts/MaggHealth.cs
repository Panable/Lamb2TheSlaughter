using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MaggHealth : Health
{
    public ParticleSystem hurtParticles;
    public CapsuleCollider collider;
    Vector3 particleLocation;

    public AudioSource audioSource;
    public AudioClip cry_04;

    public override void OnDeath()
    {
        Instantiate(hurtParticles, particleLocation, transform.localRotation);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        particleLocation = transform.TransformPoint(collider.center);
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        base.TakeDamage(amount);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, particleLocation, transform.localRotation);
        audioSource.PlayOneShot(cry_04, 60f);
    }

}
