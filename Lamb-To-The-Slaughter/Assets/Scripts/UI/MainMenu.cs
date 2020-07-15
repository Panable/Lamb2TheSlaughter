using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour //Lachlan
{
    //Audio for BGM and SFX + source to play them on
    public AudioClip startClip;
    public AudioClip leaveClip;
    public AudioSource main;

    //UI elements
    public GameObject mainMenuUI;
    public GameObject loadingMenu;

    //Buttons for play and exit
    public GameObject playgame;
    public GameObject exitgame;

    //Set the time to normal, confine the cursor, get audiosource and set menuUI active
    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        main = GetComponent<AudioSource>();
        mainMenuUI.SetActive(true);
        loadingMenu.SetActive(false);
    }

    //Depending on what screen/button is in view either play game or quit game.
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

    // When button is pressed, Play SFX and Load scene 
    public void playGame()
    {
        main.volume = 0.5f;
        main.PlayOneShot(startClip);
        SceneManager.LoadScene(2);
    }

    //When button is pressed the game will play a SFX and quit and print to the console, "Game Quit" if applicable.
    public void quitGame()
    {
        main.volume = 0.1f;
        main.PlayOneShot(leaveClip);
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
