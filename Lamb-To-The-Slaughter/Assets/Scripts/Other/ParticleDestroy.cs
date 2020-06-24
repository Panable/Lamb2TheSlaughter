using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float timer;
    public ParticleSystem sparks;
    public ParticleSystem flash;
    public GameObject anchor;

    // Start is called before the first frame update
    void Awake()
    {
        anchor = GameObject.FindGameObjectWithTag("ShootAnchor");
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
