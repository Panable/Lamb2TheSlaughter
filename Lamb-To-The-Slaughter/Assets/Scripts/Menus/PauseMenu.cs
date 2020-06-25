using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour //Lachlan
{
    public GameObject pauseMenu;
    public GameObject controlsGuide;
    public GameObject optionsMenu;
    public GameObject gameplayUI;
    bool pauseMenuEnabled;

    WeaponSelect ws;
    PlayerHealth ph;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (ws == null || ph == null)
        {
            ws = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSelect>();
            ph = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        }
            

        if (Input.GetButtonDown("Cancel"))
        {
            if (pauseMenuEnabled == false)
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
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void Start()
    {
        ws = player.GetComponent<WeaponSelect>();
        ph = player.GetComponent<PlayerHealth>();
    }

}
