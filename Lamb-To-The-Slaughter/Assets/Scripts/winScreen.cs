using UnityEngine;
using TMPro;

public class winScreen : MonoBehaviour //Lachlan
{
    public TMP_Text timerTxt;
    public float timerValue;

    // Set Timer value and allow for buttons to be pressed by unlocking cursor
    void Start()
    {
        timerValue = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GUI>().timer;
        timerTxt.SetText(timerValue.ToString());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
