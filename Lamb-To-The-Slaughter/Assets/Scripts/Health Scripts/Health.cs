using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public abstract class Health : MonoBehaviour //Dhan
{
    /// <summary>
    /// Abstract class for health
    /// </summary>
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

    //Manage dmg taking
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

    //managing death
    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    //setting health
    public void SetHealth(float amount)
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }

}
