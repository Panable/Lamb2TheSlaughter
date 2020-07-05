using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour //Lachlan
{
    public GameObject endDoor;
    public GameObject winScreen;
    public GameObject gameplayUI;

    private void Awake() //Find WinScreen and disable it (so it doesn't appear when loading)
    {
        winScreen = GameObject.FindGameObjectWithTag("WinScreen");
        winScreen.SetActive(false);
    }

    public void Update() 
    {
        if (!endDoor) //Find the ending door in boss room
        {
            endDoor = GameObject.FindGameObjectWithTag("end");
            Debug.Log("Finding EndingDoor");
            return;
        }

        if (!winScreen) //Find the winScreen incase Awake function fails.
        {
            winScreen = GameObject.FindGameObjectWithTag("WinScreen");
            Debug.Log("Finding Win Screen Brb");
            return;
        }
        
        if (endDoor.GetComponent<EndDoor>().endDoorEntered == true) //If gone through the final door.
        {
            //This is what happens when the player goes through the win door after defeating the boss. Activates winscreen, deactivates gameplayUI.
            gameplayUI.SetActive(false);
            winScreen.SetActive(true);
        }
    }
}
