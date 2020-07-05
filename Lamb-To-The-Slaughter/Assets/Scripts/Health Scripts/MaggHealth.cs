using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MaggHealth : Health //Ansaar
{
    public ParticleSystem hurtParticles;
    public CapsuleCollider collider;
    public Transform particleLocation;
    private AudioSource audioSourceM;
    public AudioClip cryM;

    Vector3 angryScale = new Vector3(1.5f, 1.5f, 1.5f);
    public bool unharmed = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        audioSourceM = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!unharmed)
        {
            gameObject.transform.localScale = Vector3.Lerp(transform.localScale, angryScale, 1 * Time.deltaTime);
        }
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        audioSourceM.PlayOneShot(cryM, 20f);
        base.TakeDamage(amount);

        unharmed = false;
        //add shit you want after damage is taken here
        Instantiate(hurtParticles, particleLocation.position, transform.localRotation);
    }

}
