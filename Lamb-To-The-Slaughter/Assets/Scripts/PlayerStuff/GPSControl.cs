using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSControl : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Vector3 cameraPos;
    private Vector3 playerPos;
    private Camera GPScam;
    [SerializeField] GameObject vignette;
    public Transform player;
    Vector3 vignetteScale;
    #endregion

    //Initialisation
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GPScam = GetComponent<Camera>();
        vignetteScale = vignette.transform.localScale;
    }

    //Controls GPS Camera's Rotation
    void Update()
    {
        playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        cameraPos = new Vector3(playerPos.x, gameObject.transform.position.y, playerPos.z);

        gameObject.transform.position = cameraPos;

        if (!player)
        {
           return; 
        }

        Zoom();
    }

    //Zoom in and out on the GPS
    void Zoom()
    {
        float zoomControl = Input.GetAxis("Mouse ScrollWheel");

        if (zoomControl != 0f)
        {
            GPScam.orthographicSize += zoomControl;
            if (GPScam.orthographicSize < 50)
            {
                GPScam.orthographicSize = 50;
            }
            else if (GPScam.orthographicSize > 100)
            {
                GPScam.orthographicSize = 100;
            }
        }
    }
}
