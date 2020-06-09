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

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        main = GetComponent<AudioSource>();
    }

    // When button is pressed, Load the scene 
    public void playGame()
    {
        main.volume = 0.1f;
        main.PlayOneShot(startClip);
        SceneManager.LoadScene("Baas'sLevel");
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
