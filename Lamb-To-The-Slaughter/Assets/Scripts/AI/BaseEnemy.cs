using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseEnemy : Component
{
    [HideInInspector]
    public GameObject obj;
    public float movementSpeed;
    public float jumpHeight;
    public float damage;
    public new ClassName name;

    IBaseStats baseStats;

    public enum ClassName { Elephant }
    public interface IBaseStats
    {
        float MovementSpeed { get; }
        float JumpHeight { get; }
        Canvas ClassHUD { get; }
        ClassName Name { get; }

    }
    public void InstantiateBaseStats()
    {
        movementSpeed = baseStats.MovementSpeed;
        jumpHeight = baseStats.JumpHeight;
        name = baseStats.Name;
    }

    Rigidbody rb;

    //Don't create enemies using this constructor. Only for changing universal variables.
    public BaseEnemy() { }

    public BaseEnemy(IBaseStats baseStats, GameObject obj)
    {
        this.baseStats = baseStats;
        InstantiateBaseStats();
        //this.obj = obj;
    }

    public virtual void Awake()
    {
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("MainAttack"))
        {
            Ability();
        }
    }


    public abstract void Ability();
    private void ActivateHUD()
    {
        Destroy(GameObject.FindGameObjectWithTag("PlayerHUD"));
        Instantiate(baseStats.ClassHUD);
    }

    public virtual void OnDeath()
    {
        GameObject.Destroy(obj);
    }

    public float GetRemainingHealth()
    {
        return obj.GetComponent<Health>().currentHealth;
    }

    public void IncreaseAndRestoreHealth(float amount)
    {
        obj.GetComponent<Health>().SetHealth(amount);
    }

}
