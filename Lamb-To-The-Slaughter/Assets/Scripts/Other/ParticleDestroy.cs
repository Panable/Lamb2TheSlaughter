using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float timer;
    public ParticleSystem sparks;
    public ParticleSystem flash;
    public Transform anchor;

    // Start is called before the first frame update
    void Awake()
    {
        anchor = GameObject.FindGameObjectWithTag("ShootAnchor").transform;
        transform.parent = anchor.gameObject.transform;

        sparks.Play();
        flash.Play();
        Invoke("DestroyParticle", timer);
    }

    // Update is called once per frame
    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
