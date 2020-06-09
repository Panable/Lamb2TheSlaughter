using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnemySelection : MonoBehaviour
{
    public GameObject Skulk;
    public GameObject Magg;
    public GameObject Crawler;
    public GameObject Husk;

    public Vector2Int SkulkChance;
    public Vector2Int MaggChance;
    public Vector2Int CrawlerChance;
    public Vector2Int HuskChance;

    int chanceValue;
    bool hasSpawned = false;


    // Start is called before the first frame update
    void Awake()
    {
        chanceValue = Random.Range(0, 11);
        //s`Debug.Log("CV: " + chanceValue);

        if (chanceValue >= SkulkChance.x && chanceValue <= SkulkChance.y)
        {
            Instantiate(Skulk, gameObject.transform.position, Quaternion.identity);
            hasSpawned = true;
        }
        else if (chanceValue > MaggChance.x && chanceValue <= MaggChance.y)
        {
            Instantiate(Magg, gameObject.transform.position, Quaternion.identity);
            hasSpawned = true;
        }
        else if (chanceValue > HuskChance.x && chanceValue <= HuskChance.y)
        {
            Instantiate(Husk, gameObject.transform.position, Quaternion.identity);
        }
        else if (chanceValue > CrawlerChance.x && chanceValue <= CrawlerChance.y)
        {
            Instantiate(Crawler, gameObject.transform.position, Quaternion.identity);
            hasSpawned = true;
        }

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
