using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Res_Shockwave : MonoBehaviour //Ansaar
{
    Vector3 waveRange = new Vector3(200, 200, 100);
    public float waveSpeed;
    Renderer rend;
    Collider col;
    public GameObject shockwaveParticles;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<MeshCollider>();
        Instantiate(shockwaveParticles, transform.position, Quaternion.Euler(-90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,1), 90 * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, waveRange, waveSpeed * Time.deltaTime);

        if (transform.localScale.x > 100)
        {
            rend.material.color -= new Color(0, 0, 0, 0.01f);
            if (rend.material.color.a < 0.05)
            {
                col.enabled = false;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<CharacterController>().isGrounded)
            {
                other.gameObject.GetComponent<Health>().TakeDamage(20f);
            }
            else if (!other.GetComponent<CharacterController>().isGrounded)
            {
                return;
            }
        }
    }
}
