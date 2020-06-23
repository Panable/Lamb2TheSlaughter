using UnityEngine;

public class winScreen : MonoBehaviour
{
    public bool win;
    public GameObject details;
    public GameObject endGameDoor;

    private void Start()
    {
        endGameDoor = GameObject.Find("EndGame");
        win = endGameDoor.GetComponent<WinGame>().gameWon;
    }

    // Update is called once per frame
    void Update()
    {
        if (win == true)
        {
            details.SetActive(true);
            Debug.Log("Did");
        }
    }
}
