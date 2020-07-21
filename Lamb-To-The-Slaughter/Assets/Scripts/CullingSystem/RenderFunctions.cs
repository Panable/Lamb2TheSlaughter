using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFunctions : MonoBehaviour //Ansaar
{
    [SerializeField] bool notInView;
    [SerializeField] PlayerCheck playerCheck;

    //Find reference
    private void Awake()
    {
        playerCheck = transform.parent.gameObject.GetComponentInChildren<PlayerCheck>();
    }

    //Set bool when visible
    private void OnBecameVisible()
    {
        notInView = false;
    }

    //Set bool when invisible
    private void OnBecameInvisible()
    {
        notInView = true;
    }

    //Regulate layer rendering
    private void Update()
    {
        if (notInView)
        {
            if (playerCheck.playerPresent)
            {
                GetComponent<Renderer>().renderingLayerMask = 1;
            }
            else
            {
                GetComponent<Renderer>().renderingLayerMask = 0;
            }
        }
        else
        {
            GetComponent<Renderer>().renderingLayerMask = 1;
        }
    }
}
