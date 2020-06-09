using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    //Damage Feedback Varables
    public Image overlay;
    Color safeColour = new Color(0f, 0f, 0f, 0f);
    Color hurtColour = new Color(1f, 1f, 1f, 0.4f);
    bool getDamage;
    public CameraShake cs;

    //for death screen
    public GameObject player;
    public GameObject deathScreen;
    public Camera deathCamera;

    //UI Variables
    public float healthValue;

    public override void OnDeath()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        DamageOverlayControl();

        healthValue = base.currentHealth;
    }

    public override void TakeDamage(float amount)
    {
        //we are taking dmg here
        base.TakeDamage(amount);

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
}
