using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolSetButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler //Ansaar
{
    #region Variables
    [SerializeField] Image sprite;
    [SerializeField] RectTransform anchor;
    [SerializeField] TMP_Text centreText;
    [SerializeField] Color unselected;
    [SerializeField] Color selected;
    [SerializeField] bool highlight = false;
    [SerializeField] Vector3 scaleVector = new Vector3(1.15f, 1.15f, 1.15f);
    [SerializeField] Inventory inventory;
    #endregion

    // Start is called before the first frame update
    void OnEnable()
    {
        highlight = false;
    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log(gameObject);
        Debug.ClearDeveloperConsole();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlight = false;
        Debug.ClearDeveloperConsole();
    }

    void CentreTextControl()
    {
        
    }

}
