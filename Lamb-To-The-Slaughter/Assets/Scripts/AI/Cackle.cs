using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cackle : MonoBehaviour //Ansaar
{
    public Animator anim;
    public Transform player;
    Vector3 rotationFix;
    public SphereCollider hitBox;
    float distToPlayer;
    float hitRange = 60f;
    float baseTriggerRadius = 0.005f;
    float attackTriggerRadius = 0.02f;

    void OnEnable()
    {
        // Gets the enemy, Finds and targets the players location.
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        //Looks at player, ignores y position
        rotationFix = player.position;
        rotationFix = new Vector3(rotationFix.x, 0, rotationFix.z);
        transform.LookAt(rotationFix);
    }

    void Update()
    {
        //Measure distance
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

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(15f);
        }
    }
}
