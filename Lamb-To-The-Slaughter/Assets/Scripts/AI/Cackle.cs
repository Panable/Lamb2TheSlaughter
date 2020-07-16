using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cackle : MonoBehaviour //Ansaar
{
    #region Variables
    [SerializeField]
    private Vector3 rotationFix;
    private float distToPlayer;
    private float hitRange = 60f;
    private float baseTriggerRadius = 0.005f;
    private float attackTriggerRadius = 0.02f;
    private Vector3 startPos;
    public SphereCollider hitBox;
    public Animator anim;
    public Transform player;
    #endregion

    //Find the player
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPos = transform.position;
    }

    //Look towards the player
    private void FixedUpdate()
    {
        startPos = transform.position;
        rotationFix = player.position;
        rotationFix = new Vector3(rotationFix.x, 0, rotationFix.z);
        transform.LookAt(rotationFix);
    }

    //Use the distance from the player to determine whether to attack or not
    void Update()
    {
        distToPlayer = FindDistance(player.transform, gameObject.transform);

        if (distToPlayer <= hitRange)
        {
            anim.SetBool("Attack", true);
            hitBox.radius = attackTriggerRadius;
        }
        else if (distToPlayer >= hitRange)
        {
            anim.SetBool("Attack", false);
            hitBox.radius = baseTriggerRadius;
        }
    }

    //Returns the distance between two points
    float FindDistance(Transform a, Transform b)
    {
        float temp = (a.position - b.position).sqrMagnitude;
        return temp;
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Health>().TakeDamage(15f);
        }
    }
}
