using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerHealth : Health
{
    public ParticleSystem hurtParticles;

    public override void OnDeath()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        base.TakeDamage(amount);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, transform.localPosition, transform.rotation);
    }

}