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
        Spawn();
    }

    // Update is called once per frame
    public void Spawn()
    {
        chanceValue = Random.Range(0, 9);
        float sizeValue = Random.Range(1f, 1.3f);

        GameObject enemy = Instantiate(Enemies[chanceValue], transform.position, Quaternion.identity);
        enemy.transform.localScale *= sizeValue;
        hasSpawned = true;

        if (hasSpawned)
        {
            Destroy(gameObject);
        }
    }
}
