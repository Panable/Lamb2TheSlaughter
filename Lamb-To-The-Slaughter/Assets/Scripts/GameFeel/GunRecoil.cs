using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public Vector3 originalPos;
    public Vector3 originalRot;

    // Start is called before the first frame update
    public IEnumerator Recoil(float duration, float magnitude)
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation.eulerAngles;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float z = Random.Range(-0.1f, -0.08f) * magnitude;
            float xRot = Random.Range(20f, 25f) * magnitude;

            transform.localPosition = Vector3.Lerp(new Vector3(originalPos.x, originalPos.y, z), originalPos, duration * Time.deltaTime);
            transform.localEulerAngles = Vector3.Lerp(new Vector3(originalRot.x - xRot, originalRot.y, originalRot.z), originalRot, duration * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
        transform.localEulerAngles = originalRot;
    }
}
