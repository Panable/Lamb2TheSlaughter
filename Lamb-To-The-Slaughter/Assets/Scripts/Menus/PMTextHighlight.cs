using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PMTextHighlight : MonoBehaviour
{
    public TMP_Text pauseMenuOption;

    Color lowlighted = new Color(0.6981f, 0.1679f, 0.1679f);
    Color highlighted = new Color(0.1777f, 0.8018f, 0.5832f);

    bool isHighlighted;

    // Start is called before the first frame update
    void Start()
    {
        isHighlighted = false;
        pauseMenuOption.color = lowlighted;
    }

    public void Highlight()
    {
        pauseMenuOption.color = highlighted;
    }

    public void LowLight()
    {
        pauseMenuOption.color = lowlighted;
    }
}
