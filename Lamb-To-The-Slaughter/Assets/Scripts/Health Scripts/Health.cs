using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Health : MonoBehaviour //Dhan
{
    public float maxHealth;
    public float currentHealth;

    [SerializeField]
    private bool regenHealth;
    public float regenRate;
    public float timeTillRegen;

    //How long to wait to regen after taking dmg
    private float regenTimer = 0.00f;
    private bool activateRegenTimer = false;

    protected virtual void Start()
    {
    }

    public virtual void TakeDamage(float amount)
    {
        regenTimer = 0.00f;
        if ((currentHealth - amount) <= 0)
        {
            OnDeath();
        }
        else
        {
            currentHealth -= amount;
        }
    }

    public abstract void OnDeath();

    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetHealth(float amount)
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }

}
