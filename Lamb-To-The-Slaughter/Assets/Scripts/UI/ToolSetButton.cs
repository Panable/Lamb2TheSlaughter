using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolSetButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler //Ansaar
{
    #region Variables
    [SerializeField] Color selected;
    [SerializeField] Vector3 scaleVector = new Vector3(1.15f, 1.15f, 1.15f);
    [SerializeField] Color numberColor;
    [SerializeField] Inventory inventory;
    [SerializeField] WeaponSelect weaponSelect; 
    [SerializeField] string toolType;

    public bool highlight = false;
    public Image sprite;
    public RectTransform anchor;
    public bool clicked = false;
    public bool buttonActive;
    public int quantity;
    public Color unselected;
    public TMP_Text centreText;
    #endregion

    // Start is called before the first frame update
    void OnEnable()
    {
        highlight = false;
        buttonActive = false;
    }

    void OnDisable()
    {
        highlight = false;
        buttonActive = false;
    }

    void FindType()
    {
        switch (toolType)
        {
            case "MedPack":
                quantity = inventory.medpack;
                break;
            case "Gas":
                quantity = inventory.gasBomb;
                break;
            case "Gravity":
                quantity = inventory.gravityBomb;
                break;
            case "Teleport":
                quantity = inventory.teleportBomb;
                break;
            case "Explosive":
                quantity = inventory.explosionBomb;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FindType();

        if (highlight)
        {
            sprite.color = Color.Lerp(sprite.color, selected, 20f * Time.deltaTime);
            anchor.localScale = Vector3.Lerp(anchor.localScale, scaleVector, 20f * Time.deltaTime);
        }
        else
        {
            sprite.color = Color.Lerp(sprite.color, unselected, 10f * Time.deltaTime);
            anchor.localScale = Vector3.Lerp(anchor.localScale,Vector3.one, 10f * Time.deltaTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlight = true;
        buttonActive = true;
        Debug.ClearDeveloperConsole();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight = false;
        buttonActive = false;
        Debug.ClearDeveloperConsole();
    }

    public void BombSelected()
    {
        clicked = true;
    }
}
