using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Rendering;

public class computepen : MonoBehaviour //NEEDS COMMENTING
{

    public BoxCollider roomprefab1;
    public BoxCollider roomprefab2;
    public Vector3 direction;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        LayerMask xd = LayerMask.NameToLayer("room");
        Debug.Log(xd.value);
        Collider[] colliders = Physics.OverlapSphere(new Vector3(50,50,50), 100f, 1 << 10);
        Debug.Log((colliders[0].bounds.max - colliders[0].bounds.center).magnitude);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
