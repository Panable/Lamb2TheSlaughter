using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RoomCollider : MonoBehaviour
{
    RoomManager rm;
    bool spawnedEnemies = false;
    bool enemiesLocated = false;
    public List<ProceduralEnemySelection> enemySpawners;

    public void InitiateBattle()
    {
        rm.LockDoors();
        SpawnEnemies();
    }

    public void SpawnEnemies()
    { 
        FindEnemySpawners();
        foreach (ProceduralEnemySelection spawner in enemySpawners)
        {
            StartCoroutine(spawner.Spawn());
        }
        spawnedEnemies = true;
        StartCoroutine(CheckForEnemies());
    }

    public void FindEnemySpawners()
    {
        foreach (Transform child in transform.parent)
        {
            if (child.CompareTag("EnemySpawner"))
            {
                enemySpawners.Add(child.GetComponent<ProceduralEnemySelection>());
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rm = transform.parent.GetComponent<RoomManager>();
        if (rm.spawnRoom) return;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CheckForEnemies()
    {
        Debug.Log("Checking");
        enemiesLocated = true;
        while (enemiesLocated)
        {
            enemiesLocated = false;
            foreach (Transform child in transform.parent)
            {
                if (child.CompareTag("Enemy"))
                {
                    enemiesLocated = true;
                    break;
                }
            }
            if (!enemiesLocated)
            {
                rm.UnlockDoors();
                StopAllCoroutines();
            }


            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (spawnedEnemies) return;
        if (other.CompareTag("Player"))
        {
            InitiateBattle();
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

}
