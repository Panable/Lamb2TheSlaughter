using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Chest : MonoBehaviour
{
    //Chest Functionality
    Animator anim;
    Collider col;
    public bool toolAccessible;
    int num;
    public GameObject activeTool;
    public GameObject medPack;

    //Inventory
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip chestCreak;

    public GameObject[] chestContents;

    // Start is called before the first frame update
    void Start()
    {
        //activeGuide.gameObject.SetActive(false);
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
            }
            else
            {
                chestContents[i].SetActive(false);
            }
        }
    }

    void ToolKillDelay()
    {
        toolAccessible = true;
    }

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
            if (activeTool.CompareTag("Bomb_Explosive"))
            {
                player.GetComponent<Inventory>().explosionBomb++;
                activeTool.SetActive(false);
            }
            if (activeTool.CompareTag("Bomb_Teleport"))
            {
                player.GetComponent<Inventory>().teleportBomb++;
                activeTool.SetActive(false);
            }
            if (activeTool.CompareTag("Bomb_Gas"))
            {
                player.GetComponent<Inventory>().gasBomb++;
                activeTool.SetActive(false);
            }
            if (activeTool.CompareTag("Bomb_Gravity"))
            {
                player.GetComponent<Inventory>().gravityBomb++;
                activeTool.SetActive(false);
            }

            medPack.SetActive(false);
            player.GetComponent<Inventory>().medpack++;
            col.enabled = false;
        }
    }
}
