using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controlsGuide;
    public GameObject optionsMenu;
    public GameObject gameplayUI;
    bool pauseMenuEnabled;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuEnabled == false)
            {
                gameplayUI.SetActive(false);
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                pauseMenuEnabled = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (pauseMenuEnabled == true)
            {
                gameplayUI.SetActive(true);
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                pauseMenuEnabled = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void ResumeGame()
    {
        gameplayUI.SetActive(true);
        optionsMenu.SetActive(false);
        controlsGuide.SetActive(false);
        pauseMenu.SetActive(false);
        pauseMenuEnabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    public void ControlsGuide()
    {
        optionsMenu.SetActive(false);
        controlsGuide.SetActive(true);
    }

    public void OptionsMenu()
    {
        controlsGuide.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void EscapeAsylum()
    {
        SceneManager.LoadScene("MainMenu");
        //Application.Quit();
        //Debug.Log("Quit Game");
    }
}
