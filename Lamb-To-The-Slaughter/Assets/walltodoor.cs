using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walltodoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter md = GetComponent<MeshFilter>();
        //Destroy(spawnDoor.gameObject);
        md.sharedMesh = ProceduralManager.doormesh.sharedMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
