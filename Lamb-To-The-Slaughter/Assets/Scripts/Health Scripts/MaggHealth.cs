using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MaggHealth : Health //Ansaar(Particles) & Lachlan(Audio)
{
    #region Variables
    public GameObject deathParticles;
    public CapsuleCollider collider;
    public Vector3 particleLocation;
    public AudioSource audioSourceM;
    public AudioClip cryM;
    public Vector3 angryScale = new Vector3(1.5f, 1.5f, 1.5f);
    public bool unharmed = true;
    #endregion

    //Properties for when killed
    public override void OnDeath()
    {
        GameObject particles = Instantiate(deathParticles, particleLocation, Quaternion.identity);
        particles.transform.localScale = angryScale / 2;
        Destroy(gameObject);
    }

    //Initialisation
    protected override void Start()
    {
        base.Start();
        audioSourceM = GetComponent<AudioSource>();
    }

    //Death & Roam/Rage check
    void Update()
    {
        if (!unharmed)
        {
            gameObject.transform.localScale = Vector3.Lerp(transform.localScale, angryScale, 1 * Time.deltaTime);
        }

        if (base.currentHealth < 1)
        {
            OnDeath();
        }
    }

    //Properties for when taking damage
    public override void TakeDamage(float amount)
    {
        audioSourceM.PlayOneShot(cryM, 20f);
        base.TakeDamage(amount);
        Instantiate(deathParticles, particleLocation, transform.rotation);
        unharmed = false;
    }

}
