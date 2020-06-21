using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Resposdine : MonoBehaviour
{
	//animator control variables (NOT IN SCRIPT)
	//float moveSpeed
	//int/float health
	//bool canMelee
	//int strikeCount
	//bool AOEattack
	//bool spawnAttack
	//int skulksSpawned
	//bool shootAttack
	//float shootDuration

	//components to be found
	Animator anim;
	public SphereCollider meleeTrigger;
	ResHealth rH;
	NavMeshAgent resAI;


	//Variables & public prefabs
	public GameObject skulk; //needed to spawn
	public Rigidbody projectile; //needed to shoot
	public ParticleSystem deathParticles; //resposdine's death
	int strikeCount; //Melee attack regulator
	bool canMelee; //player is close enough
	int battleStage; //moveset regulator
    float rHealth; //health value for this script
	public float AOEtimer; //How often it does AOE attack
	Vector3 originPos; //Centre of the room
	GameObject player; //The player lmao
	float agentToDestDist; //How far away the agent is from their destination
	public GameObject shockwave; //Shockwave attack
	public Transform projectileAnchor; //Where to shoot from
    public float projectileForce; //How strong to shoot
	float shootDelay = 1f; //How frequently to shoot
	float shootTimer; //How long it takes to shoot
	float maxShootingTime; //max amount of time it spends shooting
	bool spawnSkulk; //it can spawn a skulk
	Transform skulkAnchor; //where to spawn a skulk from
    float spawnTimer; //How long to spawn
	float spawnDelay = 1f; //How fast to spawn
	int skulkCount; //how many skulks are in the room
	Rigidbody playerRb;


    void Awake()
	{
		anim = GetComponent<Animator>();
		resAI = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
		playerRb = player.GetComponent<Rigidbody>();
		rH = GetComponent<ResHealth>();

	    strikeCount = 0;
		AOEtimer = 15f;
		meleeTrigger.radius = 17;
		originPos = transform.position;
		resAI.isStopped = false;
	}

	void FixedUpdate()
	{
		transform.LookAt(player.transform.position);

		if (resAI.isStopped == false)
		{
			anim.SetFloat("moveSpeed", 1f);
		}
		else if (resAI.isStopped == true)
		{
			anim.SetFloat("moveSpeed", 0f);
		}
	}


	void Update()
	{
		Debug.Log(AOEtimer);

		rHealth = rH.health;

		anim.SetFloat("health", rHealth);
		anim.SetInteger("strikeCount", strikeCount);
		anim.SetFloat("shootingDuration", maxShootingTime);
		anim.SetBool("canMelee", canMelee);

		StageOne();

		if (rHealth >= 50 && rHealth < 79)
		{
			maxShootingTime = 5f;
			maxShootingTime -= Time.deltaTime;
			if (maxShootingTime > 0)
			{
				ProjectileAttack();
			}
			else if (maxShootingTime < 0.1f)
			{
				StageTwo();
			}
		}
		if (rHealth >= 1 && rHealth < 49)
		{
			maxShootingTime = 10f;
			maxShootingTime -= Time.deltaTime;
			if (maxShootingTime > 0)
			{
				ProjectileAttack();
			}
			else if (maxShootingTime < 0.1f)
			{
				StageThree();
			}
		}

		if (rH.isDead)
		{
			deathParticles.Play();
			Invoke("KillBoss", 0.2f);
		}
	}

	void KillBoss()
	{
		Destroy(gameObject);
	}

	void StageOne()
	{
		strikeCount = 1;

		resAI.destination = player.transform.position;

		if (canMelee)
		{
			Invoke("PlayerPushback", 0.1f);
			canMelee = false;
		}

		AOEtimer -= Time.deltaTime;
		if (AOEtimer < 0)
		{
			strikeCount = strikeCount + 10;
			float originalStoppingDistance = resAI.stoppingDistance;
			resAI.stoppingDistance = 0f;
			resAI.speed = 8;
			resAI.destination = originPos;
			agentToDestDist = Vector3.Distance(transform.position, originPos);
			if (agentToDestDist < 0.2)
			{
				resAI.isStopped = true;
				transform.LookAt(player.transform.position);
				anim.SetBool("AOEattack", true);

				Instantiate(shockwave, transform.position, transform.localRotation);
				Debug.Log("Shockwave Spawned");

				resAI.isStopped = false;
				resAI.stoppingDistance = originalStoppingDistance;

				Invoke("AOEreset", 0.1f);
			}
		}
	}

    void AOEreset()
    {
		resAI.speed = 3;
		AOEtimer = 15f;
		anim.SetBool("AOEattack", false);
		resAI.isStopped = false;
		resAI.destination = player.transform.position;
		strikeCount = strikeCount - 10;
	}

	void StageTwo()
	{
		strikeCount = 1;
		AOEtimer = 20f;

		resAI.destination = player.transform.position;

		if (strikeCount > 2)
		{
			strikeCount = 1;
		}
	
	    if (canMelee)
		{
			anim.SetBool("CanMelee", true);
		    strikeCount += 1;
			anim.SetBool("canMelee", false);
	    }

		AOEtimer -= Time.deltaTime;
		if (AOEtimer < 0)
		{
			float originalStoppingDistance = resAI.stoppingDistance;
			resAI.stoppingDistance = 0f;
			resAI.destination = originPos;
			agentToDestDist = Vector3.Distance(transform.position, originPos);
			if (agentToDestDist < 0.2)
			{
				anim.SetBool("AOEattack", true);

				Instantiate(shockwave, transform.position, transform.localRotation);

				resAI.stoppingDistance = originalStoppingDistance;
	
				AOEtimer = 15f;
			}
			resAI.destination = player.transform.position;
		}

	}

	void StageThree()
	{
		strikeCount = 1;
		AOEtimer = 25f;

		resAI.destination = player.transform.position;

		if (strikeCount > 3)
		{
			spawnSkulk = true; //*
			strikeCount = 1;
		}
	
	    if (canMelee)
		{
			anim.SetBool("canMelee", true);
		    strikeCount += 1;
			Invoke("PlayerPushback", 0.1f);
			anim.SetBool("CanMelee", false);
	    }

		AOEtimer -= Time.deltaTime;
		if (AOEtimer < 0 && spawnSkulk != true)
		{
			float originalStoppingDistance = resAI.stoppingDistance;
			resAI.stoppingDistance = 0f;
			resAI.speed *= 2;
			resAI.destination = originPos;
			agentToDestDist = Vector3.Distance(transform.position, originPos);
			if (agentToDestDist < 0.2)
			{
				anim.SetBool("AOEattack", true);

				Instantiate(shockwave, transform.position, transform.localRotation);


				resAI.stoppingDistance = originalStoppingDistance;
	
				resAI.speed /= 2;
				AOEtimer = 15f;
			}
			resAI.destination = player.transform.position;
		}

		if (spawnSkulk)
		{
			resAI.isStopped = true;
			anim.SetBool("spawnAttack", true);
			spawnTimer = spawnDelay;
			spawnTimer -= spawnDelay;
			if (spawnTimer <= 0)
			{
				Instantiate(skulk, skulkAnchor.position, skulkAnchor.rotation); ;
				spawnTimer = spawnDelay;
			}

			skulkCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
			skulkCount -= 1;

			if (skulkCount > 9)
			{
				anim.SetBool("spawnAttack", false);
				spawnSkulk = false;
			}
		}
	}

	void ProjectileAttack()
	{
		shootTimer = shootDelay;
		shootTimer -= Time.deltaTime;
		if (shootTimer <= 0)
		{
			anim.SetBool("shootAttack", true);
			//Rigidbody projectileInstance = Instantiate(projectileAnchor.transform)
			//projectileInstance.velocity = projectileForce * projectileAnchor.forward; //Fix with AIE
			shootTimer = shootDelay;
		}
	}

	void OnTriggerEnter(Collider other) //If player enters the meleeTrigger
	{
		if (other.tag == "Player")
	    {
			Debug.Log("Entered");
			canMelee = true;
			meleeTrigger.radius = 1f;
			Invoke("TriggerReset", 0.2f);
		}
	}

    void TriggerReset()
    {
		meleeTrigger.radius = 18;
    }
}
