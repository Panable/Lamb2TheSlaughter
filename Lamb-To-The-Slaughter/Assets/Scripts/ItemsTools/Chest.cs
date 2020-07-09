using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour //Ansaar
{
    #region Variables
    private Animator anim;
    private Collider col;
    private int num;
    private string toolTag;

    public bool toolAccessible;
    public GameObject activeTool;
    public GameObject medPack;
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip chestCreak;
    public GameObject[] chestContents;
    #endregion

    //Initialisation
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();

        num = Random.Range(0, chestContents.Length);
        activeTool = chestContents[num];
        activeTool.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            if (chestContents[i] == activeTool)
            {
                activeTool.SetActive(true);
                toolTag = activeTool.tag;
            }
            else
            {
                chestContents[i].SetActive(false);
            }
        }
    }

    //Make tools accessible
    void ToolKillDelay()
    {
        toolAccessible = true;
    }

    //Pick Up Tool & Medpack
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && Input.GetButtonDown("Interact"))
        {

            anim.SetBool("openChest", true);
            audioSource.PlayOneShot(chestCreak, 20f);
            Invoke("ToolKillDelay", 0.2f);

        }

        if (toolAccessible && Input.GetButtonDown("Interact"))
        {
            switch (toolTag)
            {
                case "Bomb_Explosive":
                    player.GetComponent<Inventory>().explosionBomb++;
                    activeTool.SetActive(false);
                    break;
                case "Bomb_Teleport":
                    player.GetComponent<Inventory>().explosionBomb++;
                    activeTool.SetActive(false);
                    break;
                case "Bomb_Gas":
                    player.GetComponent<Inventory>().gasBomb++;
                    activeTool.SetActive(false);
                    break;
                case "Bomb_Gravity":
                    player.GetComponent<Inventory>().gravityBomb++;
                    activeTool.SetActive(false);
                    break;
            }

            medPack.SetActive(false);
            player.GetComponent<Inventory>().medpack++;
            col.enabled = false;
        }
    }
}
