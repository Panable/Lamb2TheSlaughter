using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour //Lachlan
{

    public AudioClip startClip;
    public AudioClip leaveClip;
    public AudioSource main;
    public GameObject mainMenuUI;
    public GameObject loadingMenu;

    public GameObject playgame;
    public GameObject exitgame;

    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        main = GetComponent<AudioSource>();
        mainMenuUI.SetActive(true);
        loadingMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButton("Submit"))
        {
            if (playgame.activeInHierarchy == true)
            {
                playGame();
            }
            if (exitgame.activeInHierarchy == true)
            {
                quitGame();
            }
        }
    }

    // When button is pressed, Load the scene 
    public void playGame()
    {
        main.volume = 0.5f;
        main.PlayOneShot(startClip);
        SceneManager.LoadScene(1);
    }

    //When button is pressed the game will quit and print to the console, "Game Quit"
    public void quitGame()
    {
        main.volume = 0.1f;
        main.PlayOneShot(leaveClip);
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
