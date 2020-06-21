using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : Health
{
    //Damage Feedback Varables
    public Image overlay;
    Color safeColour = new Color(0f, 0f, 0f, 0f);
    Color hurtColour = new Color(1f, 1f, 1f, 0.4f);
    bool getDamage;
    public CameraShake cs;
    [Header("Damage Delay Timer")]
    public double delayTimer;

    [Header("Other")]
    //for death screen
    public GameObject player;
    public GameObject deathScreen;
    public Camera deathCamera;

    //When Health is over 100/overdrive function
    WeaponSelect ws;
    public static bool overDrive = false;

    //UI Variables
    public float healthValue;
    public TMP_Text healthText;
    string ogText;
    public Gradient textColor;

    //Audio
    public AudioSource audioSourceP;
    public AudioClip[] playerCries;

    public override void OnDeath()
    {
        //ws.AOEcA.intensity.Override(0.161f);
        //ws.AOEv.color.Override(ws.originalV);
        deathScreen.SetActive(true);
        Destroy(player);
        Instantiate(deathCamera);
        Time.timeScale = 0; 
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        base.Start();
        overlay.color = safeColour;
        audioSourceP = GetComponentInChildren<AudioSource>();
        //ws.AOEv.color.Override(ws.originalV); 
    }

    // Update is called once per frame
    void Update()
    {
        DamageOverlayControl();
        deathCheck();

        healthValue = base.currentHealth;

        if (delayTimer >= 0)
        {
            delayTimer -= Time.deltaTime;
        }

        if (currentHealth > maxHealth)
        {
            overDrive = true;
            healthText.color = textColor.Evaluate(1);
            healthText.fontSize = 60;
            healthText.SetText("Overdose");
        }
        else
        {
            overDrive = false;
            healthText.color = textColor.Evaluate(0);
            healthText.fontSize = 70;
            healthText.SetText("Health");
        }
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        if (delayTimer <= 0)
        {
            audioSourceP.clip = playerCries[Random.Range(0, playerCries.Length)];
            audioSourceP.loop = false;
            audioSourceP.Play();
            base.TakeDamage(amount);
            delayTimer = 0.2;
        }

        //add shit you want after damage is taken here
        getDamage = true;
    }

    void DamageOverlayControl()
    {
        if (getDamage)
        {
            StartCoroutine(cs.Shake(0.3f, 1.5f));
            overlay.color = Color.Lerp(overlay.color, hurtColour, 20 * Time.deltaTime);
            if (overlay.color.a >= 0.39)
            {
                getDamage = false;
            }
        }
        if (!getDamage)
        {
            overlay.color = Color.Lerp(overlay.color, safeColour, 5 * Time.deltaTime);
        }
    }

    void deathCheck()
    {
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
}
