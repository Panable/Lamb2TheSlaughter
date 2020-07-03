using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnemySelection : MonoBehaviour
{
    int chanceValue;
    bool hasSpawned = false;
    public GameObject spawnParticle;
    public GameObject[] Enemies;

    [Header("Particle Scales")]
    public float scaleFactor;
    [Tooltip("Order:\n-Cackles\n-Crawlers\n-Husks\n-Maggs\n-Resposdine\n-Skulks")]
    public Vector3[] particleScales;

    private void Start()
    {

    }

    // Start is called before the first frame update
    void Update()
    {

    }

    public static float waitBeforeSpawn = 1.5f;

    // Update is called once per frame
    public IEnumerator Spawn()
    {
        chanceValue = Random.Range(0, 9);
        float sizeValue = Random.Range(1f, 1.3f);

        GameObject particles = Instantiate(spawnParticle, transform.position, Quaternion.Euler(-90,0,0));
        particles.transform.localScale = ParticleScaleConfiguration(Enemies[chanceValue]) * scaleFactor;

        yield return new WaitForSeconds(waitBeforeSpawn);

        GameObject enemy = Instantiate(Enemies[chanceValue], transform.position, Quaternion.identity, transform.parent);
        enemy.transform.localScale *= sizeValue;
        hasSpawned = true;

        if (hasSpawned)
        {
            Destroy(gameObject);
        }
    }

    Vector3 ParticleScaleConfiguration(GameObject enemy)
    {
        switch (enemy.name)
        {
                case "Cackle":
                    return particleScales[0];
                case "Crawler":
                    return particleScales[1];
                case "Husk":
                    return particleScales[2];
                case "Magg":
                    return particleScales[3];
                case "Resposdine":
                    return particleScales[4];
                case "Skulk(Fixed)":
                    return particleScales[5];
        }

        return particleScales[0];
    }
}