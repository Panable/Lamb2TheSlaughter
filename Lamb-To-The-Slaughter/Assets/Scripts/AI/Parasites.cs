using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasites : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private bool dissolveOn;
    private float dissolveControl;
    private Material dissolve;
    private GameObject matHolder;
    private Renderer rend;

    public bool DissolvedOut;
    #endregion

    //Initialisation
    void Start()
    {
        matHolder = transform.GetChild(1).gameObject;
        rend = matHolder.GetComponent<Renderer>();
        dissolve = rend.material;
        dissolveControl = 1;
        DissolvedOut = false;
    }

    //Control the dissolve
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

    //Called via Room Manager Class to dissolve in
    public void DissolveIn()
    {
        dissolveOn = true;
    }

    //Called via Room Manager Class to dissolve out
    public void DissolveOut()
    {
        dissolveOn = false;
    }
}
