using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Chest : MonoBehaviour
{
    Animator anim;
    Collider col;
    public Transform contentAnchor;
    public bool toolAccessible;
    int num;
    GameObject activeTool;
    public GameObject medPack;
    public TMP_Text activeGuide;

    //Inventory
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip chestCreak;


    public GameObject[] chestContents;
    // Start is called before the first frame update
    void Start()
    {
        activeGuide.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();
        col = GetComponent<SphereCollider>();

        num = Random.Range(0, chestContents.Length);
        activeTool = chestContents[num];
        chestContents[num].gameObject.SetActive(true);

        foreach (GameObject tool in chestContents)
        {
            if (tool.activeSelf == false)
            {
                Destroy(tool);
            }
        }
    }

    void ToolKillDelay()
    {
        toolAccessible = true;
    }

    private void OnTriggerStay(Collider other)
    {

       if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("openChest", true);
            audioSource.PlayOneShot(chestCreak, 60f);
            Invoke("ToolKillDelay", 0.2f);

        }

       if (toolAccessible && Input.GetKeyDown(KeyCode.E))
        {
            activeGuide.gameObject.SetActive(false);

            if (activeTool.tag == "Bomb_Explosive")
            {
                player.GetComponent<Inventory>().explosionBomb++;
                Destroy(activeTool);
            }
            else if (activeTool.tag == "Bomb_Teleport")
            {
                player.GetComponent<Inventory>().teleportBomb++;
                Destroy(activeTool);
            }
            else if (activeTool.tag == "Bomb_Gas")
            {
                player.GetComponent<Inventory>().gasBomb++;
                Destroy(activeTool);
            }
            else if (activeTool.tag == "Bomb_Gravity")
            {
                player.GetComponent<Inventory>().gravityBomb++;
                Destroy(activeTool);
            }
            else if (activeTool.tag == "MedPack")
            {
                player.GetComponent<Inventory>().medpack++;
                Destroy(activeTool);
            }
            Destroy(medPack);
            player.GetComponent<Inventory>().medpack++;
            col.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            activeGuide.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activeGuide.gameObject.SetActive(true);
        }
    }
}
