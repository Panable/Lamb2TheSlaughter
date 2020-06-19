﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public abstract class Health : MonoBehaviour //Dhan
{
    public float maxHealth;
    public float currentHealth;

    [SerializeField]
    private bool regenHealth;
    public float regenRate;
    public float timeTillRegen;

    [Header("Audio")]
    public AudioClip cry;
    public AudioSource audioSource;

    //How long to wait to regen after taking dmg
    private float regenTimer = 0.00f;
    private bool activateRegenTimer = false;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(cry, 70f);
            currentHealth -= amount;
        }
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public void SetHealth(float amount)
    {
        maxHealth = amount;
        currentHealth = maxHealth;
    }

}
