using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulkHealth : Health
{
    public ParticleSystem hurtParticles;
    public SphereCollider hitBox;
    Vector3 particleLocation;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        base.TakeDamage(amount);

        //add shit you want after damage is taken here
        particleLocation = transform.TransformPoint(hitBox.center);
        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }

}

