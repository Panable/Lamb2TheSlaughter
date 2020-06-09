using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform armature;

    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        
        float elapsed = 0.0f;

        while (elapsed < duration)
        {

            float x = Random.Range(-0.2f, 0.2f) * magnitude;

            transform.localPosition = new Vector3(x, originalPos.y, originalPos.z);
            magnitude = Mathf.Lerp(magnitude, 0f, duration);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
