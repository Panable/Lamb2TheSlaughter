using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour //Ansaar
{
    #region Variables
    public float timer;
    public ParticleSystem sparks;
    public ParticleSystem flash;
    public GameObject anchor;
    public string anchorHolder;
    #endregion

    //Initialisation
    void Awake()
    {
        if (anchorHolder == "Player")
        {
            anchor = GameObject.FindGameObjectWithTag("ShootAnchor");
        }
        else if (anchorHolder == "Skulk")
        {
            anchor = GameObject.FindGameObjectWithTag("SkullHead");
        }
        else if (anchorHolder == "Resposdine")
        {
            anchor = GameObject.FindGameObjectWithTag("ResAnchor");
        }

        sparks.Play();
        flash.Play();
        Invoke("DestroyParticle", timer);
    }

    //Set particle anchor
    private void Update()
    {
        transform.position = anchor.transform.position;
    }

    //Destory particle
    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
