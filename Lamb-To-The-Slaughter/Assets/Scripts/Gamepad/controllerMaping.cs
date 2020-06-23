using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerMaping : MonoBehaviour
{
    public bool IsLeft, IsRight, IsUp, IsDown;
    private float _LastX, _LastY;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float x = Input.GetAxis("DPad X");
        float y = Input.GetAxis("DPad Y");

        IsLeft = false;
        IsRight = false;
        IsUp = false;
        IsDown = false;

        if (_LastX != x)
        {
            if (x == -1)
                IsLeft = true;
            else if (x == 1)
                IsRight = true;
        }

        if (_LastY != y)
        {
            if (y == -1)
                IsDown = true;
            else if (y == 1)
                IsUp = true;
        }

        _LastX = x;
        _LastY = y;

        if (IsLeft == true)
        {
            Debug.Log("Left");

        }

        if (IsRight == true)
        {
            Debug.Log("Right");
        }

        if (IsDown == true)
        {
            Debug.Log("Down");
        }

        if (IsUp == true)
        {
            Debug.Log("Up");
        }
    }
}

