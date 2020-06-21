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

    public static float waitBeforeSpawn = 0.5f;

    // Update is called once per frame
    public IEnumerator Spawn()
    {
        chanceValue = Random.Range(0, 9);
        float sizeValue = Random.Range(1f, 1.3f);


        //add your particle system.

        yield return new WaitForSeconds(waitBeforeSpawn);

        GameObject enemy = Instantiate(Enemies[chanceValue], transform.position, Quaternion.identity, transform.parent);
        enemy.transform.localScale *= sizeValue;
        hasSpawned = true;

        //destroy particle system

        if (hasSpawned)
        {
            Destroy(gameObject);
        }
    }
}
    