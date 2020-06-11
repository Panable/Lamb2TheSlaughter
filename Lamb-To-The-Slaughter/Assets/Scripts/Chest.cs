﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public GameObject[] chestContents;
    // Start is called before the first frame update
    void Start()
    {
        activeGuide.gameObject.SetActive(false);

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
            Invoke("ToolKillDelay", 0.2f);

        }

       if (toolAccessible && Input.GetKeyDown(KeyCode.E))
        {
            activeGuide.gameObject.SetActive(false);

            if (activeTool.tag == "Bomb_Explosive")
            {
                //Increase inventory value
                Destroy(activeTool);
            }
            else if (activeTool.tag == "Bomb_Teleport")
            {
                //Increase inventory value
                Destroy(activeTool);
            }
            else if (activeTool.tag == "Bomb_Gas")
            {
                //Increase inventory value
                Destroy(activeTool);
            }
            else if (activeTool.tag == "Bomb_Gravity")
            {
                //Increase inventory value
                Destroy(activeTool);
            }
            else if (activeTool.tag == "MedPack")
            {
                //Increase inventory value
                Destroy(activeTool);
            }

            //increase Medpack's inventory value as all chests have one.
            Destroy(medPack);

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
