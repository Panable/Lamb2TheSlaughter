using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskHealth : Health //Dhan
{
    public ParticleSystem hurtParticles;
    public BoxCollider collider;
    Vector3 particleLocation;
    private AudioSource audioSourceH;
    public AudioClip cryH;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        audioSourceH = GetComponent<AudioSource>();
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
        audioSourceH.PlayOneShot(cryH, 20f);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }

}
