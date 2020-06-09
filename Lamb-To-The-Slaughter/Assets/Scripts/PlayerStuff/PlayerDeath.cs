using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
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
        Destroy(player);
    }

    //void FixedUpdate()
  //  {
    //    if (player.GetComponent<Health>().currentHealth <= 0)
      //  {
        //    UI.SetActive(false);
          //  deathScreen.SetActive(true);
        //}
    //}
}
