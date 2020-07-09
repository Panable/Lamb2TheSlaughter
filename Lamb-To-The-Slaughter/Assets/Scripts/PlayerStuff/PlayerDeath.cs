using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour //NEEDS COMMENTING
{
    public GameObject UI;
    public GameObject deathScreen;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UI.SetActive(false);
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.SetActive(false);
    }
    
    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
