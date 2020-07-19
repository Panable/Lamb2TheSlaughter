using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScream : MonoBehaviour //Ansaar
{
    [SerializeField] Vector3 waveRange;
    [SerializeField] float waveSpeed;
    Renderer rend;
    [SerializeField] GameObject shockwaveParticles;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        transform.localScale = Vector3.Lerp(transform.localScale, waveRange, waveSpeed * Time.deltaTime);
        Instantiate(shockwaveParticles, transform.position, Quaternion.Euler(90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1), 90 * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, waveRange, waveSpeed * Time.deltaTime);

        rend.material.color -= new Color(0, 0, 0, 0.01f);
        if (rend.material.color.a < 0.01)
        {
            Destroy(gameObject);
        }
    }
}
