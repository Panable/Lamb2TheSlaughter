using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSControl : MonoBehaviour
{

    public Transform player;
    Vector3 cameraPos;
    Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
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
