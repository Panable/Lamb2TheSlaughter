using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMessages : MonoBehaviour
{
    //Manager properties
    public GameObject[] bMplane;
    public Texture[] bMtexture;
    Renderer bMplaneRenderer;

    //For convenience reasons, teleport bomb limit function also occurs here
    [SerializeField] private GameObject[] foundTpBombs;

    // Start is called before the first frame update
    void Awake()
    {
        //TeleportBombLimit();

        bMplane = GameObject.FindGameObjectsWithTag("BloodMessage");

        ShuffleArray(bMplane);

        int[] usedTextures = new int[bMtexture.Length];
        for (int i = 0; i < bMtexture.Length; i++)
        {
            usedTextures[i] = 0;
        }
        foreach (GameObject messageAnchor in bMplane)
        {
            int num = Random.Range(0, bMtexture.Length);

            while (usedTextures[num] != 0)
            {
                num++;
                if(num >= bMtexture.Length)
                {
                    num = 0;
                }
            }
            usedTextures[num] = 1;

            if (num < 1)
            {
                break;
            }

            bMplaneRenderer = messageAnchor.GetComponent<Renderer>();
            bMplaneRenderer.material.mainTexture = bMtexture[num];
            bMplaneRenderer.tag = "Untagged";
        }

        KillExcess();
    }

    // Update is called once per frame
    void KillExcess()
    {
        bMplane = GameObject.FindGameObjectsWithTag("BloodMessage");
        foreach (GameObject messageAnchor in bMplane)
        {
            messageAnchor.SetActive(false);
        }
    }

    void ShuffleArray(GameObject[] planeAnchors)
    {
        for (int i = planeAnchors.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            GameObject tmp = planeAnchors[i];
            planeAnchors[i] = planeAnchors[r];
            planeAnchors[r] = tmp;
        }
    }

    void TeleportBombLimit()
    {
        foundTpBombs = GameObject.FindGameObjectsWithTag("Bomb_Teleport");
        if (foundTpBombs.Length > 1)
        {
            foundTpBombs[0].tag = "Untagged";
            foreach (GameObject removedBomb in foundTpBombs)
            {
                if (removedBomb.tag == "Bomb_Teleport")
                {
                    removedBomb.SetActive(false);
                }
            }
        }
    }

}
