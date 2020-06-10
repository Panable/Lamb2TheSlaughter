using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKill : MonoBehaviour
{
    public float timer;
    // Start is called before the first frame update
    void Awake()
    {
        Invoke("DestroyParticle", timer);
    }

    // Update is called once per frame
    void DestroyParticle()
    {
        Destroy(gameObject);
    }
}
