using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMessages : MonoBehaviour
{
    public GameObject[] bMplane;
    public Texture[] bMtexture;
    Renderer bMplaneRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        bMplane = GameObject.FindGameObjectsWithTag("BloodMessage");
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
}
