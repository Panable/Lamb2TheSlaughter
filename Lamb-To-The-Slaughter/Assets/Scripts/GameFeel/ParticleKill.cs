using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKill : MonoBehaviour //Ansaar
{
    public float timer;

    //Initialisation
    void Awake()
    {
        Invoke("DestroyParticle", timer);
    }

    //Destroy Particles
    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
