using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    public float timer;

    void OnEnable()
    {
        timer = 8.2f;
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 8.1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
