using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResProjectile : MonoBehaviour
{
    public Vector3 hitScale;
    Rigidbody rb;
    SphereCollider sc;
    public ResHealth rHealth;
    GameObject resposdine;
    Renderer rend;
    public GameObject flares;

    // Start is called before the first frame update
    void Awake()
    {
        resposdine = GameObject.FindGameObjectWithTag("Enemy");
        rHealth = resposdine.GetComponent<ResHealth>();
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rHealth == null)
        {
            rend.material.color += new Color(0.1f, 0.1f, 0.1f, -0.02f);
            sc.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        transform.localScale = hitScale;
        Destroy(flares);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        sc.isTrigger = true;
        sc.radius = 0.4f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(5f);
        }
    }
}
