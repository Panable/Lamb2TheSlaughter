using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class TextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler   //Ansaar
{
    public TMP_Text polaroidLabel;

    Color lowlighted = new Color(0.1309434f, 0.02064792f, 0.02064792f);
    Color highlighted = new Color(0.4245283f, 0.04205233f, 0.04205233f);

    // Start is called before the first frame update
    void Start()
    {
        polaroidLabel.color = lowlighted;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        polaroidLabel.color = highlighted;
        Debug.ClearDeveloperConsole();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        polaroidLabel.color = lowlighted;
        Debug.ClearDeveloperConsole();
    }

}
