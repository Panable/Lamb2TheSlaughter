using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolManager : MonoBehaviour //Ansaar
{
    [SerializeField] ToolSetButton[] buttons;
    [SerializeField] List<ToolSetButton> inactiveButtons;
    [SerializeField] Color numberColor;
    [SerializeField] WeaponSelect weaponSelect;

    public TMP_Text centreText;
    public List<ToolSetButton> activeButtons;
    public bool bombThrown;
    public TMP_Text text;

    //Set tool select menu to default when enabling
    void OnEnable()
    {
        centreText.gameObject.SetActive(true);
        centreText.SetText("Tools");
        centreText.color = Color.white;
        centreText.fontSize = 36;

        for (int i = 0; i < buttons.Length; i++)
        {
            inactiveButtons.Add(buttons[i]);
            buttons[i].sprite.color = buttons[i].unselected;
            buttons[i].anchor.localScale = Vector3.one;
            buttons[i].highlight = false;
        }
    }

    //Clear list when disabled
    private void OnDisable()
    {
        inactiveButtons.Clear();
        centreText.gameObject.SetActive(false);
    }

    //Keep track of active buttons to control centre text
    void Update()
    {
        for (int i = 0; i < inactiveButtons.Count; i++)
        {
            if (inactiveButtons[i].buttonActive)
            {
                activeButtons.Add(inactiveButtons[i]);
                inactiveButtons.Remove(inactiveButtons[i]);
            }
        }

        for (int j = 0; j < activeButtons.Count; j++)
        {
            if (!activeButtons[j].buttonActive)
            {
                inactiveButtons.Add(activeButtons[j]);
                activeButtons.Remove(activeButtons[j]);
            }
        }

        if (activeButtons.Count == 0)
        {
            centreText.SetText("Tools");
            centreText.color = Color.white;
            centreText.fontSize = 36;
        }
        else
        {
            centreText.SetText(activeButtons[0].quantity.ToString());
            centreText.color = numberColor;
            centreText.fontSize = 72;

            if(activeButtons[0].clicked)
            {
                weaponSelect.toolsetControl = false;
            }
        }
    }
}
