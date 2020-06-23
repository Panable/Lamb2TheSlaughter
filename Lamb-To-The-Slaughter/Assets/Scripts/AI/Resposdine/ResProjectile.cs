using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResProjectile : MonoBehaviour
{
    public Vector3 hitScale;
    Rigidbody rb;
    SphereCollider sc;
    public Color fadeOut;
    public ResHealth rHealth;
    bool isDead = false;
    public Material mat;
    Renderer rend;
    public Material baseMat;
    bool bubbleHit = false;
    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        rend = GetComponent<Renderer>();
        baseMat.color = mat.color;
        baseMat.SetColor("_EmissionColor", mat.color);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        isDead = rHealth.isDead;

        if (bubbleHit)
        {
            player.GetComponent<Health>().TakeDamage(5f);
            bubbleHit = false;
        }

        if (rHealth == null)
        {
            baseMat.color = Color.Lerp(baseMat.color, fadeOut, 5 * Time.deltaTime);
            baseMat.SetColor("_EmissionColor", baseMat.color);
            sc.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        transform.localScale = hitScale;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        sc.isTrigger = true;
        sc.radius = 0.4f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(5f);
            //bubbleHit = true;
        }
    }
}
