using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour //Lachlan
{
    public float timer;

    //Set Timer to 8.2 seconds.
    void OnEnable()
    {
        timer = 8.2f;
    }

    //Update timer to match real time. When the timer finishes/reaches 0, load the scene manager, reset the timer.
    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 8.2f;
            SceneManager.LoadScene("MainMenu");
        }
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
