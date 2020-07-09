using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler   //Ansaar
{
    #region Variables
    [SerializeField]
    private Color lowlighted = new Color(0.1309434f, 0.02064792f, 0.02064792f);
    private Color highlighted = new Color(0.4245283f, 0.04205233f, 0.04205233f);

    public TMP_Text polaroidLabel;
    #endregion

    //Initialisation
    void Start()
    {
        polaroidLabel.color = lowlighted;
    }

    //Highlight when cursor is over text
    public void OnPointerEnter(PointerEventData eventData)
    {
        polaroidLabel.color = highlighted;
        Debug.ClearDeveloperConsole();
    }

    //Lowlight when cursor is not over text
    public void OnPointerExit(PointerEventData eventData)
    {
        polaroidLabel.color = lowlighted;
        Debug.ClearDeveloperConsole();
    }

}
