using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skulkProjectile : MonoBehaviour
{
    Rigidbody rb;
    SphereCollider col;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void KillProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Health>().TakeDamage(2f);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Untagged")
        {
            col.isTrigger = false;
            rb.useGravity = false;
            Invoke("KillProjectile", 5f);
        }
    }
}
