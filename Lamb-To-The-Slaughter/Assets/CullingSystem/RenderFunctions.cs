using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFunctions : MonoBehaviour
{
    [SerializeField] bool notInView;
    [SerializeField] PlayerCheck playerCheck;

    private void Awake()
    {
        playerCheck = transform.parent.gameObject.GetComponentInChildren<PlayerCheck>();
    }

    private void OnBecameVisible()
    {
        notInView = false;
    }

    private void OnBecameInvisible()
    {
        notInView = true;
    }

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
