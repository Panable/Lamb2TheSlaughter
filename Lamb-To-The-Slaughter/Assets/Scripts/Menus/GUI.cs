﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI : MonoBehaviour //Lachlan
{
    GameObject player;
    bool dead;

    //for time alive
    public TMP_Text timertxt;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateTimer();
    }

    //Time alive timer function.
    void updateTimer()
    {
        if (player)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timertxt.SetText(string.Format("{0:0}:{1:00}", minutes, seconds)); 
        }
        //Add stop timer too if the player dies, display time on death/win screen.
        if (player == null)
        {
            dead = true;
        }
    }
}