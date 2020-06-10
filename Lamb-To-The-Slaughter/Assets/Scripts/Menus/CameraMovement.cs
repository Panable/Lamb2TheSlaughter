using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour //Ansaar
{
    public Transform startGame;
    public Transform options;
    public Transform blackPrism;
    public Transform exitGame;
    float panSpeed = 5f;

    public GameObject startGameButton;
    public GameObject exitGameButton;
    public GameObject musicButtonPlus;
    public GameObject musicButtonMinus;
    public GameObject soundButtonPlus;
    public GameObject soundButtonMinus;
    public GameObject graphicButtonPlus;
    public GameObject graphicButtonMinus;

    float distance;

    int posCount;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = startGame.transform.position;
        Vector3 newRotation = startGame.transform.rotation.eulerAngles;
        gameObject.transform.eulerAngles = newRotation;
        posCount = 1;
        startGameButton.SetActive(true);
        exitGameButton.SetActive(false);
        musicButtonPlus.SetActive(false);
        musicButtonMinus.SetActive(false);
        soundButtonPlus.SetActive(false);
        soundButtonMinus.SetActive(false);
        graphicButtonPlus.SetActive(false);
        graphicButtonMinus.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PositionCount();
        //Debug.Log(posCount);


        if (posCount == 5)
        {
            posCount = 1;
        }
        else if (posCount == 0)
        {
            posCount = 4;
        }

        if (posCount == 2)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, options.transform.position, Time.deltaTime * panSpeed);
            Vector3 newRotation = options.transform.rotation.eulerAngles;
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, newRotation, Time.deltaTime * panSpeed);
            startGameButton.SetActive(false);
            exitGameButton.SetActive(false);

            distance = Vector3.Distance(transform.position, options.transform.position);
            if (distance < 0.04)
            {
                musicButtonPlus.SetActive(true);
                musicButtonMinus.SetActive(true);
                soundButtonPlus.SetActive(true);
                soundButtonMinus.SetActive(true);
                graphicButtonPlus.SetActive(true);
                graphicButtonMinus.SetActive(true);
          
            }
        }

        if (posCount == 1)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, startGame.transform.position, Time.deltaTime * panSpeed);
            Vector3 newRotation = startGame.transform.rotation.eulerAngles;
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, newRotation, Time.deltaTime * panSpeed);
            startGameButton.SetActive(true);
            exitGameButton.SetActive(false);
            musicButtonPlus.SetActive(false);
            musicButtonMinus.SetActive(false);
            soundButtonPlus.SetActive(false);
            soundButtonMinus.SetActive(false);
            graphicButtonPlus.SetActive(false);
            graphicButtonMinus.SetActive(false);
        }

        if (posCount == 3)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, blackPrism.transform.position, Time.deltaTime * panSpeed);
            Vector3 newRotation = blackPrism.transform.rotation.eulerAngles;
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, newRotation, Time.deltaTime * panSpeed);
            startGameButton.SetActive(false);
            exitGameButton.SetActive(false);
            musicButtonPlus.SetActive(false);
            musicButtonMinus.SetActive(false);
            soundButtonPlus.SetActive(false);
            soundButtonMinus.SetActive(false);
            graphicButtonPlus.SetActive(false);
            graphicButtonMinus.SetActive(false);
        }

        if (posCount == 4)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, exitGame.transform.position, Time.deltaTime * panSpeed);
            Vector3 newRotation = exitGame.transform.rotation.eulerAngles;
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, newRotation, Time.deltaTime * panSpeed);
            startGameButton.SetActive(false);
            exitGameButton.SetActive(true);
            musicButtonPlus.SetActive(false);
            musicButtonMinus.SetActive(false);
            soundButtonPlus.SetActive(false);
            soundButtonMinus.SetActive(false);
            graphicButtonPlus.SetActive(false);
            graphicButtonMinus.SetActive(false);
        }
    }

    public void PositionCount()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            posCount = posCount + 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            posCount = posCount - 1;
        }
    }

    public void NextPage()
    {
        posCount = posCount + 1;
    }

    public void PreviousPage()
    {
        posCount = posCount - 1;
    }
}