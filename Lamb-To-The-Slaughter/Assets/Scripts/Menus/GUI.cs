﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI : MonoBehaviour //Lachlan
{
    GameObject player;

    //for time alive
    public TMP_Text timertxt;
    float timer;

    //for player health
    PlayerHealth playerHealthRef;
    public TMP_Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthRef = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateTimer();

        healthText.SetText(playerHealthRef.healthValue.ToString() + "%");
    }

    //Time alive timer function.
    void updateTimer()
    {
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timertxt.SetText(string.Format("{0:0}:{1:00}", minutes, seconds));
        //Add stop timer too if the player dies, display time on death/win screen.
    }
}