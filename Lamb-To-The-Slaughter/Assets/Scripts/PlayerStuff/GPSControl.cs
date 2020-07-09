using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSControl : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Vector3 cameraPos;
    private Vector3 playerPos;

    public Transform player;
    #endregion

    //Initialisation
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
    }
}
