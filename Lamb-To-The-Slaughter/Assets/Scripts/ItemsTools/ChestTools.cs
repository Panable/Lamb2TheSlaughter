using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTools : MonoBehaviour
{
    public Chest chestScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
       if((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E)))
        {
            Destroy(gameObject);
        }
    }
}
