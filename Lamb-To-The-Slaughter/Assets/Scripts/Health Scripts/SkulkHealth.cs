using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulkHealth : Health
{
    public ParticleSystem hurtParticles;
    public BoxCollider collider;
    Vector3 particleLocation;
    public GameObject player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        particleLocation = transform.TransformPoint(collider.center);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Health>().TakeDamage(2f);
        }
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        base.TakeDamage(amount);

        //add shit you want after damage is taken here
        Instantiate(hurtParticles, particleLocation, transform.localRotation);
    }

}

