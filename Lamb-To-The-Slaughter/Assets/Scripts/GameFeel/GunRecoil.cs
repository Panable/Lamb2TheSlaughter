using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour //Dhan
{
    #region Variables
    public Vector3 originalPos;
    public Vector3 lerpPos;
    public Vector3 currentPos;
    public float lerpSpeed = 1f;
    public float slerpSpeed = 10f;
    public float lerpBackDistance = 1f;
    public bool initialRecoil = false;
    float lerpIndex;
    float slerpIndex;
    #endregion

    //Initialisation of weapon position and lerp position
    public void Start()
    {
        originalPos = transform.localPosition;
        lerpPos = new Vector3(originalPos.x, originalPos.y, originalPos.z - lerpBackDistance);
    }

    //start recoil
    public void StartRecoil()
    {
        ResetIndexes();
        initialRecoil = true;
    }

    //initial push back recoil
    public void InitialRecoil()
    {
        //if hasn't finished lerping, lerp otherwise do the returning lerp
        if (lerpIndex <= 1)
        {
            lerpIndex += Time.deltaTime * lerpSpeed;
            currentPos = Vector3.Lerp(originalPos, lerpPos, lerpIndex);
        }
        else
        {
            ReturnPos();
        }

        //set final weapon position
        transform.localPosition = new Vector3(originalPos.x, currentPos.y, currentPos.z);
    }

    //return back recoil
    public void ReturnPos()
    {
        slerpIndex += Time.deltaTime * slerpSpeed;
        currentPos = Vector3.Slerp(currentPos, originalPos, slerpIndex);
        if (slerpIndex >= 1)
        {
            ResetIndexes();
            initialRecoil = false;
        }
    }

    //resetting current lerps
    public void ResetIndexes()
    {
        lerpIndex = 0;
        slerpIndex = 0;
    }

    private void Update()
    {
        if (initialRecoil)
        {
            InitialRecoil();
        }
    }

}
