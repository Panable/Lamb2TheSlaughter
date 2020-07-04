using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasites : MonoBehaviour
{
    [SerializeField] bool dissolveOn;
    [SerializeField] float dissolveControl;
    [SerializeField] Material dissolve;
    [SerializeField] GameObject matHolder;
    [SerializeField] Renderer rend;
    public bool DissolvedOut;

    // Start is called before the first frame update
    void Start()
    {
        matHolder = transform.GetChild(1).gameObject;
        rend = matHolder.GetComponent<Renderer>();
        dissolve = rend.material;
        dissolveControl = 1;
        DissolvedOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        dissolveControl = Mathf.Clamp(dissolveControl, 0, 1);
        dissolve.SetFloat("Vector1_F0460486", dissolveControl);

        if (dissolveOn)
        {
            dissolveControl -= 0.05f;
        }

        if (!dissolveOn)
        {
            dissolveControl += 0.05f;

            if (dissolveControl > 0.95f)
            {
                DissolvedOut = true;
            }
        }
        
    }

    public void DissolveIn()
    {
        dissolveOn = true;
    }

    public void DissolveOut()
    {
        dissolveOn = false;
    }

    public bool CanSetInactive()
    {
        return DissolvedOut;
    }
}
