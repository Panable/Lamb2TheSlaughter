using UnityEngine;
using TMPro;

public class winScreen : MonoBehaviour //Lachlan
{
    //Timer value and TXT object
    public TMP_Text timerTxt;
    public int timerValue;

    //to allow the pause screen to be destoryed
    public GameObject pauseScreen;

    // Set Timer value and allow for buttons to be pressed by unlocking cursor
    void Start()
    {
        timerValue = Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Canvas").GetComponent<GUI>().timer);
        timerTxt.SetText(timerValue.ToString() + " seconds");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Destroy PauseMenu so you can't pause when dead.
    private void LateUpdate()
    {
        if (!pauseScreen)
        {
            return;
        }
        else Destroy(pauseScreen);
    }
}
