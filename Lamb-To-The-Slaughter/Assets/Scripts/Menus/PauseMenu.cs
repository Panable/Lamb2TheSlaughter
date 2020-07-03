using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour //Lachlan
{
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

    WeaponSelect ws;
    PlayerHealth ph;
    public GameObject player;
    GameObject boss;
    GameObject[] sort;

    // Update is called once per frame
    void Update()
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
        bossUI.SetActive(false);
        ws = player.GetComponent<WeaponSelect>();
        ph = player.GetComponent<PlayerHealth>();
    }

}
