using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    //Components
    [Header("Public Components")]
	Animator anim;
	SphereCollider meleeTrigger;
	public ResHealth rH;
	NavMeshAgent resAI;

	//Melee Attack Properties
	[Header("Melee Properties")]
	public int strikeCount;
    public bool canMelee;
	public CapsuleCollider weapon;

	//Projectile Attack Properties
	[Header("Projectile Properties")]
	public Rigidbody projectile;
    public Transform projectileAnchor;
	public float projectileForce;
	public bool canShoot;
	public float maxShootingTime = 10f;
	public float maxShootingTime2 = 15f;
	public float shootDelay;
	float shootTimer;
    public GameObject shotParticles;

	//AOE Attack Properties
	[Header("AOE Properties")]
	public float debugAOEtimer;
	public GameObject shockwave;
	float timer = 10f;
	bool canShockwave;
	int numOfsW;

	//Other Related Properties
	[Header("Other Properties")]
	public ParticleSystem deathParticles;
    public float rHealth;
	public bool bossIsActive = false;

	//Combat Related Properties
	[Header("Combat Properties")]
	public int battleStage;
	public Vector3 originPos;
	public GameObject player;
	public float agentDistance;
	public bool focusPlayer;

	//Spawn Attack Properties
	[Header("Spawn Properties")]
	public bool canSpawn;
	public Transform spawnAnchor;
	public GameObject skulk;
	public GameObject skulkParticles;
	public Vector3 particleOffset;
	GameObject[] skulkCount;

	// Start is called before the first frame update
	void Awake()
    {
		bossIsActive = true;

        //Find Components & References
		anim = GetComponent<Animator>();
		resAI = GetComponent<NavMeshAgent>();
		meleeTrigger = GetComponent<SphereCollider>();
		player = GameObject.FindGameObjectWithTag("Player");

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
		//Debug.Log(battleStage);
		rHealth = rH.currentHealth;
		weapon.enabled = canMelee;

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
		anim.SetBool("shootAttack", canShoot);

		//Distinguish & Call Battle Functions
		if (rHealth <= 100 && rHealth > 80)
		{
			battleStage = 1;

			BattleStageOne();
			Shockwave(10f);
		}
		if (rHealth <= 80 && rHealth > 50)
		{
			anim.SetFloat("shootDuration", maxShootingTime);
			battleStage = 2;

			maxShootingTime -= Time.deltaTime;
			if (maxShootingTime < 0)
			{
				canShoot = false;
				canMelee = true;
				anim.SetFloat("adjustSpeed", 1f);
				BattleStageTwo();
				Shockwave(15f);
			}
            else
            {
				canShoot = true;
				canMelee = false;
				anim.SetFloat("adjustSpeed", 0.5f);
			}

			shootTimer -= Time.deltaTime;
			if (canShoot)
			{
				canMelee = false;
				canShockwave = false;
				SpawnAttack(false);
				if (shootTimer <= 0)
                {
                    TransitionStage(10, 1);
					shootTimer = shootDelay;
                }
				
			}
		}
		else if (rHealth <= 50 && rHealth > 0)
		{
			anim.SetFloat("shootDuration", maxShootingTime2);
			battleStage = 3;

			maxShootingTime2 -= Time.deltaTime;
			if (maxShootingTime2 < 0)
			{
				canShoot = false;
				anim.SetFloat("adjustSpeed", 1f);
				BattleStageThree();
				Shockwave(15f);
			}
			else
			{
				canShoot = true;
				anim.SetFloat("adjustSpeed", 1f);
			}

			shootTimer -= Time.deltaTime;
			if (canShoot)
			{
				SpawnAttack(false);
				canMelee = false;
				canShockwave = false;
				canShockwave = false;
				if (shootTimer <= 0)
				{
					TransitionStage(30, 0.5f);
					shootTimer = shootDelay;
				}
			}
		}
	}

	//Projectile attack
	void TransitionStage(float force, float delay)
    {
		shootDelay = delay;
		Rigidbody projectileInstance = Instantiate(projectile, projectileAnchor.position, projectileAnchor.localRotation);
		projectileInstance.velocity = force * projectileAnchor.forward;
		Instantiate(shotParticles, projectileAnchor.position, Quaternion.identity);
    }

    private void OnEnable()
    {
		canShoot = false;
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
		SpawnAttack(true);

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
		canMelee = false;
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
		//Melee Attack
		resAI.destination = player.transform.position;
		FindDistance(player.transform.position);

		//Randomises strike type & takes into account the battle stage
		if (canMelee)
		{
			strikeCount = Random.Range(1, strikeTypes);
			canMelee = false;
		}

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

    //Resets properties
    private void Reset()
    {
		maxShootingTime = 10f;
    }

    //Regulates how many and how frequently a shockwave is spawned
    IEnumerator InvokeShockwave(float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
			float randRot = Random.Range(0f, 90f);
			Instantiate(shockwave, transform.position, Quaternion.Euler(270f, randRot, 0));
            yield return new WaitForSecondsRealtime(interval);
        }
    }

    void SpawnAttack( bool finalStage)
    {
		skulkCount = GameObject.FindGameObjectsWithTag("Enemy");

        if (!finalStage)
        {
			if (skulkCount.Length - 1 < 1)
			{
				Instantiate(skulk, spawnAnchor.position, Quaternion.identity);
			}
		}
        else if (finalStage)
        {
			if (skulkCount.Length - 1 < 2)
			{
				Instantiate(skulk, spawnAnchor.position, Quaternion.identity);
			}
		}
	}
}
