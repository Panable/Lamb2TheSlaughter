using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Husk : MonoBehaviour //Ansaar & Dhan
{
    #region Variables
    [SerializeField]
    private bool inTrigger;
    private bool attacking = false;
    private NavMeshAgent huskAgent;
    private Transform player;
    public bool pushed;
    public Animator anim;
    #endregion

    //Initialisation
    void OnEnable()
    {
        huskAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        huskAgent.isStopped = false;
    }

    //Move the Husk w/ Appropriate animations. // Also checks if it has been pushed to reset velocity & Path
    private void Update()
    {
        if (huskAgent.isStopped)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);
        }
        else if (!huskAgent.isStopped)
        {
            huskAgent.destination = player.position;
            anim.SetBool("isMoving", true);
            anim.SetBool("isAttacking", false);
        }

        pushed = player.GetComponent<WeaponSelect>().AOEsoundplay;

        if (pushed == true)
        {
            huskAgent.isStopped = true;
            huskAgent.ResetPath();
        }
    }

    //Attack if player is in trigger
    void OnTriggerStay(Collider collision)
    {
        if (attacking) return;
        if (collision.gameObject.tag == "Player")
        {
            inTrigger = true;
            StartCoroutine(attack(0.5f, collision));
        }
    }

    //Attack Control
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inTrigger = false;
        }
    }

    //Attack Coroutine
    IEnumerator attack(float strikeTime, Collider collision)
    {
        attacking = true;
        huskAgent.isStopped = true;
        yield return new WaitForSeconds(strikeTime);
        if (inTrigger)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(10f);
        }
        huskAgent.isStopped = false;
        attacking = false;
    }
}
