using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour //Lachlan
{
    public GameObject UI;
    public GameObject deathScreen;
    public GameObject pauseScreen;
    public GameObject player;
    public bool dead;

    //Finds the player, sets the GUI to false, sets the death screen UI to active, unlock Cursor, deactive the player.
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UI.SetActive(false);
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.SetActive(false);
        dead = true;
    }

    //Destroy PauseMenu so you can't pause when dead.
    private void LateUpdate()
    {
      if (!pauseScreen)
        {
            return;
        }
        else Destroy(pauseScreen);
    }

    //When button pressed, bool dead to false and load the main menu.
    public void toMainMenu()
    {
        dead = false;
        SceneManager.LoadScene("MainMenu");
    }
}
