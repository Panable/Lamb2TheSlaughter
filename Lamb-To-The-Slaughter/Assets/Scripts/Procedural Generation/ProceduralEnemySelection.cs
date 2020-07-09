using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnemySelection : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private int chanceValue;
    private bool hasSpawned = false;

    [Tooltip("Order:\n-Cackles\n-Crawlers\n-Husks\n-Maggs\n-Resposdine\n-Skulks")]
    public Vector3[] particleScales;

    public GameObject spawnParticle;
    public GameObject[] Enemies;
    public float scaleFactor;
    public static float waitBeforeSpawn = 1.5f;
    #endregion 

    //Spawn Enemy with random chance & size
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

    //Configure the scale of the particle system based on enemy
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