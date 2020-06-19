using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnemySelection : MonoBehaviour
{
    int chanceValue;
    bool hasSpawned = false;

    //public bool InitiateSpawn = false;

    public GameObject[] Enemies;

    private void Start()
    {

    }

    // Start is called before the first frame update
    void Update()
    {

    }

    // Update is called once per frame
    public void Spawn()
    {
        chanceValue = Random.Range(0, 9);
        float sizeValue = Random.Range(1f, 1.3f);

        GameObject enemy = Instantiate(Enemies[chanceValue], transform.position, Quaternion.identity, transform.parent);
        enemy.transform.localScale *= sizeValue;
        hasSpawned = true;

        if (hasSpawned)
        {
            Destroy(gameObject);
        }
    }
}
    