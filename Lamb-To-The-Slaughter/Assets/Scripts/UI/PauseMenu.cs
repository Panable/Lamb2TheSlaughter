using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour //Lachlan + Ansaar
{
    //Pause Menu UI Elements
    public GameObject pauseMenu;
    public GameObject controlsGuide;
    public GameObject optionsMenu;
    public GameObject gameplayUI;
    bool pauseMenuEnabled;

    //BossProperties
    public GameObject bossUI;
    public TMP_Text bossHealth;
    public TMP_Text bossName;
    public bool bossIsActive;
    GameObject boss;
    GameObject[] sort;

    //Player Stuff
    WeaponSelect ws;
    PlayerHealth ph;
    public GameObject player;

    //To Disable pause menu on win and death screens
    public GameObject deathScreen;
    public GameObject winScreen;

    //Set boss to false, find components, set death and win screen to false.
    private void Awake()
    {
        bossUI.SetActive(false);
        ws = player.GetComponent<WeaponSelect>();
        ph = player.GetComponent<PlayerHealth>();
        deathScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    //Call functions every frame
    void Update()
    {
        pauseButtonPressed();
        bossUIChceck();
        findWSPH();
    }

    //Checks if the escape/cancel input is pressed, if it is pause the game by stopping time, deactivating the UI elements and activating new pause menu UI, sets the cursor to active.
    void pauseButtonPressed()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (pauseMenuEnabled == false)
            {
                if (deathScreen.activeInHierarchy == true)
                {
                    //Makes sure that the menu can't be activated when dead
                }

                if (winScreen.activeInHierarchy == true)
                {
                    //Makes sure that the menu can't be activated when won
                }

                else
                {
                    ws.AOEcA.intensity.Override(0.161f);
                    ws.AOEv.color.Override(ws.originalV);
                    gameplayUI.SetActive(false);
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                    pauseMenuEnabled = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
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

    //Find the game objects WS (weapon select) & PH (player health) to help set graphical stuff
    void findWSPH() 
    {
        if (ws == null || ph == null)
        {
            ws = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSelect>();
            ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }
    }

    //Checks if the boss is active and if they are set the bossUI to active. (Displays Boss Health)
    void bossUIChceck()
    {
        if (bossIsActive)
        {
            bossUI.SetActive(true);

            sort = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < sort.Length; i++)
            {
                if (sort[i].GetComponent<ResHealth>() != null)
                {
                    boss = sort[i];
                }
            }

            if (boss == null)
            {
                bossUI.SetActive(false);
            }
        }
    }

    //When press resume game button, set gameplay UI to active, pause menu to false, lock the cursor and set the time back to normal
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

    //When clicked, it sets active the controls guide. Options menu is disabled.
    public void ControlsGuide()
    {
        optionsMenu.SetActive(false);
        controlsGuide.SetActive(true);
    }

    //When pressed the options are shown. Control guide is disabled.
    public void OptionsMenu()
    {
        controlsGuide.SetActive(false);
        optionsMenu.SetActive(true);
    }

    //Load the game back to the main menu when pressed.
    public void EscapeAsylum()
    {
        SceneManager.LoadScene("MainMenu");  
    }

    //Quit the game
    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
