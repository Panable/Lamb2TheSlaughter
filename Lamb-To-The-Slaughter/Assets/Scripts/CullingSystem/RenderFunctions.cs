using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFunctions : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField] bool notInView;
    [SerializeField] PlayerCheck playerCheck;
    #endregion

    //Find reference from room prefab
    private void Awake()
    {
        playerCheck = transform.parent.gameObject.GetComponentInChildren<PlayerCheck>();
    }

    //Set bool when visible
    private void OnBecameVisible()
    {
        notInView = false;
    }

    //Set bool when not visible
    private void OnBecameInvisible()
    {
        notInView = true;
    }

    //Regulate which layer to render on based on visibility
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
