using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour //Lachlan
{
    //Inventory Values
    public int explosionBomb;
    public int teleportBomb;
    public int gravityBomb;
    public int gasBomb;
    public int medpack;
    public int keys;

    //Text Values
    public TMP_Text explosionBombTXT;
    public TMP_Text teleportBombTXT;
    public TMP_Text gravityBombTXT;
    public TMP_Text gasBombTXT;
    public TMP_Text medpackTXT;

    private void Update()
    {
        ToolsToText();
    }

    void ToolsToText()
    {
        explosionBombTXT.text = explosionBomb.ToString();
        teleportBombTXT.text = teleportBomb.ToString();
        gravityBombTXT.text = gravityBomb.ToString();
        gasBombTXT.text = gasBomb.ToString();
        medpackTXT.text = medpack.ToString();
    }
}
