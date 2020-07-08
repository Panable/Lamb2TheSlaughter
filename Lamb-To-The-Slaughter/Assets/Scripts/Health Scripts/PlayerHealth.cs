using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : Health
{
    //Damage Feedback Varables
    public Image overlay;
    Color safeColour = new Color(0f, 0f, 0f, 0f);
    Color hurtColour = new Color(1f, 1f, 1f, 0.4f);
    bool getDamage;
    public CameraShake cs;
    [Header("Damage Delay Timer")]
    float delayTimer = 0.2f;
    int healthToDisplay;
    bool canCountDown;
    bool isDoneCounting;
    public UnityEngine.Rendering.Universal.ColorAdjustments damageCA;
    public UnityEngine.Rendering.VolumeProfile vp;
    public Color deathWarning;
    public Color originalColor;

    [Header("Other")]
    //for death screen
    public GameObject player;
    public GameObject deathScreen;
    public Camera deathCamera;

    //When Health is over 100/overdrive function
    WeaponSelect ws;
    public static bool overDrive = false;

    //UI Variables
    public TMP_Text healthText;
    string ogText;
    public Gradient textColor;
    public TMP_Text healthValue;

    [Header("Audio")]
    //Audio
    public AudioSource audioSourceP;
    public AudioClip[] playerCries;
    public AudioSource audioSourceLowHealth;
    public AudioClip lowHealth;

    public override void OnDeath()
    {
        damageCA.colorFilter.value = originalColor;
        deathScreen.SetActive(true);
        player.SetActive(false);
        Instantiate(deathCamera);
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        base.Start();
        overlay.color = safeColour;
        healthToDisplay = (int)currentHealth;
        audioSourceP = GetComponentInChildren<AudioSource>();
        PostProcessConfiguration();
    }

    public void OnMedPackUpdate()
    {
        healthToDisplay = (int)currentHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Medpack"))
        {
            Invoke("OnMedPackUpdate", 0.2f);
        }

        UIHealthShuffle();

        healthValue.SetText(healthToDisplay.ToString() + "%");

        DamageOverlayControl();

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

        DeathCheck();
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        if (delayTimer < 0)
        {
            audioSourceP.clip = playerCries[Random.Range(0, playerCries.Length)];
            audioSourceP.loop = false;
            audioSourceP.Play();
            healthToDisplay = (int)currentHealth;
            base.TakeDamage(amount);
            canCountDown = true;
            delayTimer = 0.2f;
        }

        //add shit you want after damage is taken here
        getDamage = true;
    }

    void DamageOverlayControl()
    {
        if (getDamage)
        {
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

        if (currentHealth < 10f)
        {
            damageCA.colorFilter.value = Color.Lerp(damageCA.colorFilter.value, deathWarning, 2 * Time.deltaTime);
            audioSourceLowHealth.loop = true;
            audioSourceLowHealth.Play();
        }
        else
        {
            damageCA.colorFilter.value = Color.Lerp(damageCA.colorFilter.value, originalColor, 2 * Time.deltaTime);
            audioSourceLowHealth.Stop();
        }
    }

    void DeathCheck()
    {
        if (healthToDisplay <= 0)
        {
            OnDeath();
        }
    }

    void UIHealthShuffle()
    {
        if (canCountDown)
        {
            healthToDisplay -= 1;
            if (healthToDisplay < currentHealth)
            {
                healthToDisplay = (int)currentHealth;
                canCountDown = false;
            }
        }
    }

    void PostProcessConfiguration()
    {
        ColorAdjustments cA;

        if (vp.TryGet<ColorAdjustments>(out cA))
        {
            damageCA = cA;
        }

        originalColor = damageCA.colorFilter.value;
    }
}
