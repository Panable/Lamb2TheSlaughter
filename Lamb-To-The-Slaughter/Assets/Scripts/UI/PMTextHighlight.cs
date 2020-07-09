using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PMTextHighlight : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Color lowlighted = new Color(0.6981f, 0.1679f, 0.1679f);
    private Color highlighted = new Color(0.1777f, 0.8018f, 0.5832f);
    private bool isHighlighted;

    public TMP_Text pauseMenuOption;
    #endregion

    //Initialisation
    void Start()
    {
        isHighlighted = false;
        pauseMenuOption.color = lowlighted;
    }

    //Highlight text under cursor
    public void Highlight()
    {
        pauseMenuOption.color = highlighted;
    }

    //Lowlight text under cursor
    public void LowLight()
    {
        pauseMenuOption.color = lowlighted;
    }
}
