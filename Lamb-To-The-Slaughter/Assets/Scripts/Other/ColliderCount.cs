using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class ColliderCount : MonoBehaviour //NEEDS COMMENTING
{
    public BoxCollider boxCol;
    public int colliderAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider collider in gameObject.GetComponents<Collider>())
        {
            colliderAmount++;
        }

        Debug.Log("Amount of colliders in " + gameObject.transform.name + " is: " + colliderAmount);

        



    }

    public bool notActivated = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V) && notActivated)
        {
            Debug.Log("Attempting movement");
            this.transform.position = ExtensionMethods.ColliderToWorldPoint(boxCol);
            notActivated = false;
        }
    }
}
