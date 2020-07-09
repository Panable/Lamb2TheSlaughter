using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : Health //Ansaar(Graphics) & Lachlan(Audio & Death)
{
    #region Variables
    [SerializeField]
    private Color safeColour = new Color(0f, 0f, 0f, 0f);
    private Color hurtColour = new Color(1f, 1f, 1f, 0.4f);
    private bool getDamage;
    private float delayTimer = 0.2f;
    private int healthToDisplay;
    private bool canCountDown;
    private bool isDoneCounting;
    private ColorAdjustments damageCA;
    private WeaponSelect ws;
    private string ogText;

    public Image overlay;
    public CameraShake cs;
    public UnityEngine.Rendering.VolumeProfile vp;
    public Color deathWarning;
    public Color originalColor;
    public GameObject player;
    public GameObject deathScreen;
    public Camera deathCamera;
    public static bool overDrive = false;
    public TMP_Text healthText;
    public Gradient textColor;
    public TMP_Text healthValue;
    public AudioSource audioSourceP;
    public AudioClip[] playerCries;
    public AudioSource audioSourceLowHealth;
    public AudioClip lowHealth;
    #endregion

    //Properties for when killed
    public override void OnDeath()
    {
        damageCA.colorFilter.value = originalColor;
        deathScreen.SetActive(true);
        player.SetActive(false);
        Instantiate(deathCamera);
        Time.timeScale = 0;
    }

    //Initialisation
    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        base.Start();
        overlay.color = safeColour;
        healthToDisplay = (int)currentHealth;
        audioSourceP = GetComponentInChildren<AudioSource>();
        PostProcessConfiguration();
    }

    //GUI health display failsafe
    public void OnMedPackUpdate()
    {
        healthToDisplay = (int)currentHealth;
    }

    //Regulate Health GUI & Death Check
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

    //Properties for when taking damage
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

    //Blood overlay when taking damage
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

    //Death Check
    void DeathCheck()
    {
        if (healthToDisplay <= 0)
        {
            OnDeath();
        }
    }

    //Shuffle GUI Health
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

    //Configure Post-Processing Settings
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
