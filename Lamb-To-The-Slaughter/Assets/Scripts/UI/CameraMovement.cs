using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour //Ansaar
{
    //Camera movement for the main menu
    #region Variables
    [SerializeField]
    private float panSpeed = 5f;
    Camera cam;
    float distance;
    int posCount;

    public Transform startGame;
    public Transform options;
    public Transform blackPrism;
    public Transform exitGame;
    public GameObject startGameButton;
    public GameObject exitGameButton;
    public GameObject musicButtonPlus;
    public GameObject musicButtonMinus;
    public GameObject soundButtonPlus;
    public GameObject soundButtonMinus;
    public GameObject graphicButtonPlus;
    public GameObject graphicButtonMinus;
    #endregion

    //Initialisation
    void Start()
    {
        cam = GetComponent<Camera>();

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

    //Menu Camera Movement & Element Regulation
    void Update()
    {
        PositionCount();

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
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, panSpeed * Time.deltaTime);
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
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, panSpeed * Time.deltaTime);
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
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 70f, panSpeed * Time.deltaTime);
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
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, panSpeed * Time.deltaTime);
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

    //Determine which position the camera is in
    public void PositionCount()
    {
        if (Input.GetButtonDown("Right"))
        {
            posCount = posCount + 1;
        }
        else if (Input.GetButtonDown("Left"))
        {
            posCount = posCount - 1;
        }
    }

    //Button Function for next page
    public void NextPage()
    {
        posCount = posCount + 1;
    }

    //Button Function for precious page
    public void PreviousPage()
    {
        posCount = posCount - 1;
    }
}