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
        Debug.Log(toolTag);
    }

    //Pick Up Tool & Medpack
    private void OnTriggerStay(Collider other)
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (other.tag == "Player" && Input.GetButtonDown("Interact") && !toolAccessible)
        {

            anim.SetBool("openChest", true);
            audioSource.PlayOneShot(chestCreak, 20f);
            Invoke("ToolKillDelay", 0.2f);
            Debug.Log("Killed");
        }

        if (toolAccessible && Input.GetButtonDown("Interact"))
        {
            Debug.Log(toolTag);

            if (player.GetComponent<Inventory>() == null)
            {
                Debug.Log("NoInventory");
            }

            if (toolTag == "Bomb_Explosive")
            {
                Debug.Log("ReachedIntoIf");
                player.GetComponent<Inventory>().explosionBomb++;
                activeTool.SetActive(false);
            }
            if (toolTag == "Bomb_Teleport")
            {
                Debug.Log("ReachedIntoIf");
                player.GetComponent<Inventory>().teleportBomb++;
                activeTool.SetActive(false);
            }
            if (toolTag == "Bomb_Gas")
            {
                Debug.Log("ReachedIntoIf");
                player.GetComponent<Inventory>().gasBomb++;
                activeTool.SetActive(false);
            }
            if (toolTag == "Bomb_Gravity")
            {
                Debug.Log("ReachedIntoIf");
                player.GetComponent<Inventory>().gravityBomb++;
                activeTool.SetActive(false);
            }

            Debug.Log("Passed break");

            medPack.SetActive(false);
            player.GetComponent<Inventory>().medpack++;
            col.enabled = false;
        }
    }
}
