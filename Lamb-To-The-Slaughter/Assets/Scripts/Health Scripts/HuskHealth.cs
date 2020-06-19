using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskHealth : Health
{
    public ParticleSystem hurtParticles;
    public BoxCollider collider;
    Vector3 particleLocation;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


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
    }

}
