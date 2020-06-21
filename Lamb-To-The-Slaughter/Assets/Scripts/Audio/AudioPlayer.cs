﻿using UnityEngine;

public class AudioPlayer : MonoBehaviour //Lachlan
{
    //Audio Sources
    public GameObject player;
    public AudioSource audioSource;

    //Gun Sounds
    public AudioClip shoot;
    public AudioClip reload;
    //Health Sounds
    public AudioClip heal;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        playShootSound();
        playReloadingSound();
    }

    public void playShootSound()
    {
        if (player.GetComponent<WeaponSelect>().selectedWeapon.attack == true)
        {
            audioSource.PlayOneShot(shoot, 3f);
            player.GetComponent<WeaponSelect>().selectedWeapon.attack = false;
        }
    }

    public void playReloadingSound()
    {
        if (player.GetComponent<WeaponSelect>().selectedWeapon.reloadSFX == true)
        {
            audioSource.PlayOneShot(reload, 2f);
            player.GetComponent<WeaponSelect>().selectedWeapon.reloadSFX = false;
        }
    }

}
