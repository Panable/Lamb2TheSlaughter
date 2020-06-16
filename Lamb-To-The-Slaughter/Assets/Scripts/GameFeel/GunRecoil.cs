using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    // First time lerps back then it slerps back? (When slerping only y and z axis will be minded)

    //Local transform vectors
    public Vector3 originalPos;
    public Vector3 lerpPos; //how far back is it going to lerp after fire
    public Vector3 currentPos;

    public float lerpSpeed = 1f;
    public float slerpSpeed = 10f;

    public float lerpBackDistance = 1f;

    public bool initialRecoil = false;

    float lerpIndex;
    float slerpIndex;

    public void Start()
    {
        originalPos = transform.localPosition;
        lerpPos = new Vector3(originalPos.x, originalPos.y, originalPos.z - lerpBackDistance);
    }

    public void StartRecoil()
    {
        ResetIndexes();
        initialRecoil = true;
    }

    public void InitialRecoil()
    {
        if (lerpIndex <= 1)
        {
            lerpIndex += Time.deltaTime * lerpSpeed;
            currentPos = Vector3.Lerp(originalPos, lerpPos, lerpIndex);
        }
        else
        {
            ReturnPos();
        }

        transform.localPosition = new Vector3(originalPos.x, currentPos.y, currentPos.z);
    }
    public void ReturnPos()
    {
        Debug.Log("returning");
        slerpIndex += Time.deltaTime * slerpSpeed;
        currentPos = Vector3.Slerp(currentPos, originalPos, slerpIndex);
        if (slerpIndex >= 1)
        {
            ResetIndexes();
            initialRecoil = false;
        }
    }

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
