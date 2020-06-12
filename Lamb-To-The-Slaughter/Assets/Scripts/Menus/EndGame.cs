using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    //Activating UI Elements 
    public GameObject UI;
    public GameObject EndGameUI;
    public GameObject boss;

    //Audio
    public AudioClip victoryBGM;
    public AudioClip victorySFX;
    public AudioSource mainSource;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        UI.SetActive(false);
        EndGameUI.SetActive(true);
    }

    //Function if game is won
    void FinishedGame()
    {
        mainSource.PlayOneShot(victoryBGM);
        mainSource.PlayOneShot(victorySFX);
        EndGameUI.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void EscapeAsylum()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
