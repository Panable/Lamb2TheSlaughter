using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject UI;
    public bool gameWon;

    private void Start()
    {
        //winScreen = GameObject.Find("WinScreenUI");
        UI = GameObject.Find("GameplayUI");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameWon();
        }
    }

    public void GameWon()
    {
        gameWon = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UI.SetActive(false);
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
