using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Maggots : MonoBehaviour //Lachlan
{
    //Maggot's Gameobject components
    private NavMeshAgent maggotAgent;
    private Rigidbody maggotRB;
    public Transform player;
    MaggHealth mh;

    //Particles to spawn when damaged
    public ParticleSystem Injured;

    //Variables forwonder
    public float wanderRadius = 50;
    public float wanderTimer = 3.7f;
    private float timer;
    public float bounceThreshold;

    //Variables for bouncyboi
    float bounceHeight;
    public GameObject mainBone;
    public float colSpeed;
    public float colHeight;
    Vector3 colPos;
    public float freeze;

    //Bounce Audio
    public AudioClip bounce;
    private AudioSource audioSource;
    private bool justBounced;

    //CollisionFix
    CapsuleCollider col;

    //Finds The Componenets Neccessary for the enemy to move and finds the target to avoid.
    void OnEnable()
    {
        mh = GetComponent<MaggHealth>();
        col = gameObject.GetComponent<CapsuleCollider>();
        maggotAgent = GetComponent<NavMeshAgent>();
        maggotRB = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        colPos = col.center;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (mh.unharmed)
        {
            //Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            //maggotAgent.SetDestination(newPos);
            //timer = 0;
        }

        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Health>().TakeDamage(6f);
        }
    }

    void BounceSound()
    {
        if (justBounced == true)
        {
            audioSource.PlayOneShot(bounce, 10f);
            justBounced = false;
        }
    }

    //Update the timer to change direction when it runs out
    void Update()
    {
        if (mh.unharmed)
        {
            bouncyBoi();
            NewBouncePos();
            BounceSound();
        }
        else if(!mh.unharmed)
        {
            maggotAgent.SetDestination(player.position);
            bouncyBoi();
            BounceSound();
        }

        timer += Time.deltaTime;
    }

    public void NewBouncePos()
    {
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            maggotAgent.SetDestination(newPos);
            timer = 0;
        }

        if (maggotAgent.isStopped == true)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            maggotAgent.SetDestination(newPos);
            timer = 0;
        }
    }

    //Make Maggot bounce
    public void bouncyBoi()
    {
        RaycastHit hit;
        Ray downray = new Ray(mainBone.transform.position, -Vector3.up);

        Vector3 bouncePos = new Vector3(col.center.x, colHeight, col.center.z);

        if (Physics.Raycast(downray, out hit))
        {
            bounceHeight = hit.distance;
        }

        if (bounceHeight > bounceThreshold)
        {
            col.center = Vector3.Lerp(col.center, bouncePos, colSpeed * Time.deltaTime);
            maggotAgent.speed = 4.5f;
        }
        else if (bounceHeight < bounceThreshold)
        {
            col.center = Vector3.Lerp(col.center, colPos, colSpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                justBounced = true;
            }
            maggotAgent.speed = 0f;
        }
    }

    //Getting random location
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
