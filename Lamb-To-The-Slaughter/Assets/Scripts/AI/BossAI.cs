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
	[Header("Melee Properties")]
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
	public float debugAOEtimer;
	public GameObject shockwave;
	float timer = 10f;
	bool canShockwave;
	int numOfsW;

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
	public bool focusPlayer;

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
		resAI.isStopped = false;
		transform.LookAt(player.transform.position);
		resAI.destination = player.transform.position;
	}

    private void FixedUpdate()
    {
        //Regulates orientation
        if (focusPlayer)
        {
            transform.LookAt(player.transform.position);
        }
        else if (!focusPlayer)
        {
			transform.LookAt(originPos);
		}
    }
    // Update is called once per frame
    void Update()
    {
		Debug.Log(battleStage);
		rHealth = rH.currentHealth;

		//Animator Controls
		if (resAI.isStopped == false)
		{
			anim.SetFloat("moveSpeed", 1f);
		}
		else if (resAI.isStopped == true)
		{
			anim.SetFloat("moveSpeed", 0f);
		}
		anim.SetInteger("strikeCount", strikeCount);

		//Distinguish & Call Battle Functions
		if (rHealth <= 100 && rHealth > 80)
		{
			battleStage = 1;

			BattleStageOne();
			Shockwave(10f);
		}
		else if (rHealth <= 79 && rHealth > 50)
		{
			battleStage = 2;

			BattleStageTwo();
			Shockwave(15f);
		}
		else if (rHealth <= 49 && rHealth > 0)
		{
			battleStage = 3;

			BattleStageThree();
			Shockwave(20f);
		}
	}

    //First stage of the battle
	void BattleStageOne()
    {
		timer -= Time.deltaTime;
		debugAOEtimer = timer;
		if (timer > 0)
		{
			//Regulate Melee Attack
			focusPlayer = true;
			StrikeAttack(1);
        }
        else if (timer <= 0)
        {
			//Regulate AOE Attack
			focusPlayer = false;
			AOEattack(1);
		}
    }

    //Second stage of the battle
    void BattleStageTwo()
    {
		timer -= Time.deltaTime;
		debugAOEtimer = timer;
		if (timer > 0)
		{
			//Regulate Melee Attack
			focusPlayer = true;
			StrikeAttack(3);
		}
		else if (timer <= 0)
		{
			//Regulate AOE Attack
			focusPlayer = false;
			AOEattack(2);
		}
	}

	//Third stage of the battle
	void BattleStageThree()
	{
		timer -= Time.deltaTime;
		debugAOEtimer = timer;
		if (timer > 0)
		{
			//Regulate Melee Attack
			focusPlayer = true;
			StrikeAttack(4);
		}
		else if (timer <= 0)
		{
			//Regulate AOE Attack
			focusPlayer = false;
			AOEattack(3);
		}
	}

	//Find distance from agent to destination
	void FindDistance(Vector3 destination)
	{
		agentDistance = Vector3.Distance(transform.position, destination);
	}

    //Adaptable AOE Function
	void AOEattack(int numOfShockwaves)
    {
		numOfsW = numOfShockwaves;
        FindDistance(originPos);
		resAI.stoppingDistance = 0.1f;
	    resAI.destination = originPos;

        if (agentDistance < 0.3)
        {
			canShockwave = true;
            //Continued in update
		}
	}

	//Adaptable Melee Attack & Regulator
	void StrikeAttack(int strikeTypes)
    {
        //Randomises strike type & takes into account the battle stage
		if (canMelee)
		{
			strikeCount = Random.Range(1, strikeTypes);
		}

		//Melee Attack
		resAI.destination = player.transform.position;
		FindDistance(player.transform.position);
		if (agentDistance < resAI.stoppingDistance - 1)
		{
			canMelee = true;
		}
		else
		{
			canMelee = false;
		}

		//Animator Controls
		anim.SetBool("canMelee", canMelee);
	}

	//Spawns a shockwave (taking into account how often and how many
	void Shockwave(float numTimer)
    {
        //animator control
		anim.SetBool("AOEattack", canShockwave);

		if (canShockwave)
		{
			//resAI.isStopped = true;
			focusPlayer = true;

			StartCoroutine(InvokeShockwave(1, numOfsW));

			//Reset AOE
			resAI.isStopped = false;
			resAI.stoppingDistance = 10f;
			timer = numTimer;
			canShockwave = false;
		}
	}

    //Regulates how many and how frequently a shockwave is spawned
    IEnumerator InvokeShockwave(float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
			float randRot = UnityEngine.Random.Range(0f, 90f);
			Instantiate(shockwave, transform.position, Quaternion.Euler(transform.localRotation.x, randRot, transform.localRotation.y));
			yield return new WaitForSecondsRealtime(interval);
        }
    }
}
