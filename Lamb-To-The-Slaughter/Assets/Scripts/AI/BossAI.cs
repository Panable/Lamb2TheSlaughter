using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    //Components
	Animator anim;
	SphereCollider meleeTrigger;
	ResHealth rH;
	NavMeshAgent resAI;

    //Melee Attack Properties
    [Header ("Melee Properties")]
    public int strikeCount;
    public bool canMelee;

	//Spawn Attack Properties
	[Header("Spawn Properties")]
	public GameObject skulk;
	public bool spawnSkulk;
    public Transform skulkAnchor;
	public float spawnTimer;
	public float spawnDelay = 1f;
	public int skulkCount;

	//Projectile Attack Properties
	[Header("Projectile Properties")]
	public Rigidbody projectile;
    public Transform projectileAnchor;
	public float projectileForce;
	public float shootDelay = 1f;
	public float shootTimer;
    public float maxShootingTime;

	//AOE Attack Properties
	[Header("AOE Properties")]
	public float AOEtimer;
	public GameObject shockwave;

	//Health Related Properties
	[Header("Health Properties")]
	public ParticleSystem deathParticles;
    public float rHealth;

	//Combat Related Properties
	[Header("Combat Properties")]
	public int battleStage;
	public Vector3 originPos;
	public GameObject player;
	public float agentDistance;
	public Transform destination;

    // Start is called before the first frame update
    void Awake()
    {
        //Find Components & References
		anim = GetComponent<Animator>();
		resAI = GetComponent<NavMeshAgent>();
		meleeTrigger = GetComponent<SphereCollider>();
		player = GameObject.FindGameObjectWithTag("Player");
		rH = GetComponent<ResHealth>();

		//Initialise Properties
		originPos = transform.position;
		rHealth = rH.currentHealth;
	}

    // Update is called once per frame
    void Update()
    {
        //Idle & Float Control
		if (resAI.isStopped == false)
		{
			anim.SetFloat("moveSpeed", 1f);
		}
		else if (resAI.isStopped == true)
		{
			anim.SetFloat("moveSpeed", 0f);
		}

		//Distinguish Battle Stages
		if (rHealth <= 100 && rHealth > 80)
		{
			//Set Battle Stage Propeties
			strikeCount = 1;
			battleStage = 1;
			AOEtimer = 15;

            //Call Battle Stage
			BattleStageOne();
		}
		else if (rHealth <= 79 && rHealth > 50)
		{
			battleStage = 2;
			AOEtimer = 25;
		}
		else if (rHealth <= 49 && rHealth > 0)
		{
			battleStage = 3;
			AOEtimer = 35;
		}
	}

    void BattleStageOne()
    {
		FindDistance(player.transform.position);
        
    }

	//Find distance from agent to destination
	void FindDistance(Vector3 destination)
	{
		agentDistance = Vector3.Distance(transform.position, destination);
	}

	void 
}
