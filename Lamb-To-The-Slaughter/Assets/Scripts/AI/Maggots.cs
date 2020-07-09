using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Maggots : MonoBehaviour //Lachlan & Ansaar
{
    #region Variables
    [SerializeField]
    private NavMeshAgent maggotAgent;
    private Transform player;
    private MaggHealth mh;
    private float wanderRadius = 50;
    private float wanderTimer = 3.7f;
    private float timer;
    private float bounceThreshold = 1.15f;
    private float bounceHeight;
    private float colSpeed = 30f;
    private float colHeight = 4f;
    private Vector3 colPos;
    private CapsuleCollider col;
    private AudioSource audioSource;
    private bool justBounced;

    public AudioClip bounce;
    public GameObject mainBone;
    #endregion

    //Initialisation
    void OnEnable()
    {
        mh = GetComponent<MaggHealth>();
        col = gameObject.GetComponent<CapsuleCollider>();
        maggotAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        colPos = col.center;
    }

    //Damage the player
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<Health>().TakeDamage(6f);
        }
    }

    //Sound Control
    void BounceSound()
    {
        if (justBounced == true)
        {
            audioSource.PlayOneShot(bounce, 10f);
            justBounced = false;
        }
    }

    //Control between Roam AI & Raged AI
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

    //Set a random destination for the Roam AI
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

    //Control the hitbox to follow the magg's bounce
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

    //Find a random location on the navmesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
