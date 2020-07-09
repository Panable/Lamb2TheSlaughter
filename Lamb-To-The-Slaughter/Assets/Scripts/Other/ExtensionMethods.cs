using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods //NEEDS COMMENTING
{

    public static Vector2 Round(this Vector2 vector2, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector2(
            Mathf.Round(vector2.x * multiplier) / multiplier,
            Mathf.Round(vector2.y * multiplier) / multiplier);
    }
    public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(vector3.x * multiplier) / multiplier,
            Mathf.Round(vector3.y * multiplier) / multiplier,
            Mathf.Round(vector3.z * multiplier) / multiplier);
    }

    public static Vector2 Clamp(Vector2 value, float min, float max)
    {

        float x = Mathf.Clamp(value.x, min, max);
        float y = Mathf.Clamp(value.y, min, max);
        return new Vector2(x, y);

    }

    public static Vector3 ColliderToWorldPoint(BoxCollider boxCol)
    {
        if(boxCol == null)
        {
            Debug.LogError(boxCol.transform.parent.name);
        }
        return boxCol.transform.TransformPoint(new Vector3(boxCol.center.x, boxCol.center.y, boxCol.center.z));
    }
    public static Vector3 ColliderToWorldPoint(BoxCollider boxCol, Transform transform)
    {
        if (boxCol == null)
        {
            Debug.LogError(transform.parent.name);
        }
        return boxCol.transform.TransformPoint(new Vector3(boxCol.center.x, boxCol.center.y, boxCol.center.z));
    }

    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
          
            n--;
            int k = Random.Range(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}

