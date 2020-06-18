using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnemySelection : MonoBehaviour
{
    int chanceValue;
    bool hasSpawned = false;

    public GameObject[] Enemies;


    // Start is called before the first frame update
    void Awake()
    {
        chanceValue = Random.Range(0, 9);

        Instantiate(Enemies[chanceValue], transform.position, Quaternion.identity);
        hasSpawned = true;

        if (hasSpawned)
        {
            Destroy(gameObject);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
