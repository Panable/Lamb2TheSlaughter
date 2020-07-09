using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skulkProjectile : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Rigidbody rb;
    private SphereCollider col;
    #endregion

    //Initialisation
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
    }

    //Damage player or Kill Projectile
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
            Invoke("KillProjectile", 2f);
        }
    }

    //Invoked function to Kill Projectile
    void KillProjectile()
    {
        Destroy(gameObject);
    }
}
