using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorlockingtest : MonoBehaviour //Dhan
{
    // Start is called before the first frame update.
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
