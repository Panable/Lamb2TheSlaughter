using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float timer;
    public ParticleSystem sparks;
    public ParticleSystem flash;
    public GameObject anchor;

    //Classification Int
    public string anchorHolder;

    // Start is called before the first frame update
    void Awake()
    {
        if (anchorHolder == "Player")
        {
            anchor = GameObject.FindGameObjectWithTag("ShootAnchor");
        }
        else if (anchorHolder == "Skulk")
        {
            anchor = GameObject.FindGameObjectWithTag("SkulkAnchor");
        }
        else if (anchorHolder == "Resposdine")
        {
            anchor = GameObject.FindGameObjectWithTag("ResAnchor");
        }

        sparks.Play();
        flash.Play();
        Invoke("DestroyParticle", timer);
    }

    private void Update()
    {
        transform.position = anchor.transform.position;
    }

    // Update is called once per frame
    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
