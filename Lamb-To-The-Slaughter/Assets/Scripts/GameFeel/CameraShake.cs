using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour //Ansaar
{
    //Shake Camera
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = new Vector3(0, 1, 0);
        Vector3 originalRot = transform.localEulerAngles;
        
        float elapsed = 0.0f;

        while (elapsed < duration)
        {

            float xPos = Random.Range(-0.3f, 0.3f) * magnitude;
            float xRot = 10f * magnitude;

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(xPos, originalPos.y, originalPos.z), duration * Time.deltaTime); ;
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(originalRot.x + xRot, originalPos.y, originalPos.z), Time.deltaTime * duration);

            magnitude = Mathf.Lerp(magnitude, 0f, duration * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * duration * 2);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, originalRot, Time.deltaTime * duration * 2);
    }
}
