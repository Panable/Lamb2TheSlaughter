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
    bool canGrab;

    public bool toolAccessible;
    public GameObject activeTool;
    public GameObject medPack;
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip chestCreak;
    public GameObject[] chestContents;
    float dist;
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
        }

        if (toolAccessible && Input.GetButtonDown("Interact"))
        {
            if (toolTag == "Bomb_Explosive")
            {
                canGrab = true;
                player.GetComponent<Inventory>().explosionBomb++;
                //activeTool.SetActive(false);
            }
            if (toolTag == "Bomb_Teleport")
            {
                canGrab = true;
                player.GetComponent<Inventory>().teleportBomb++;
                //activeTool.SetActive(false);
            }
            if (toolTag == "Bomb_Gas")
            {
                canGrab = true;
                player.GetComponent<Inventory>().gasBomb++;
                //activeTool.SetActive(false);
            }
            if (toolTag == "Bomb_Gravity")
            {
                canGrab = true;
                player.GetComponent<Inventory>().gravityBomb++;
                //activeTool.SetActive(false);
            }

            canGrab = true;
            //medPack.SetActive(false);
            player.GetComponent<Inventory>().medpack++;
            col.enabled = false;
        }
    }

    void Grab(GameObject tool, Transform player)
    {
        tool.GetComponent<Collider>().enabled = false;
        tool.transform.position = Vector3.Lerp(tool.transform.position, player.position, 10 * Time.deltaTime);
    }

    private void Update()
    {
        if (canGrab)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Grab(activeTool, player.transform);
            Grab(medPack, player.transform);
            dist = (activeTool.transform.position - player.transform.position).sqrMagnitude;
            if (dist < 0.1)
            {
                Destroy(activeTool);
                Destroy(medPack);
                Destroy(this);
            }

        }
    }
}
