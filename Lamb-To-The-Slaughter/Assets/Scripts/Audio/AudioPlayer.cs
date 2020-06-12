using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioPlayer : MonoBehaviour //Lachlan
{
    //GetAllAudioSources
    public AudioSource[] allAudio;

    //Audio Sources
    [Header("Audio Sources")]
    public AudioSource GunSource;
    public AudioSource MaggSource;

    //Player SFX
    [Header("Player Sounds")]
    public GameObject player;
    //Gun Sounds
    public AudioClip shoot;
    public AudioClip reload;
    //Health Sounds
    public AudioClip heal;
    public AudioClip nearlyDead;

    //Enemy Sounds
    [Header("Enemy Sounds")]
    //Magg Enemy
    public AudioClip maggBounce;
    public AudioClip maggCry;
    public AudioClip maggDie;
    //Skulks
    public AudioClip skullksFloat;
    public AudioClip skulksCry;
    public AudioClip skullksDie;
    //Husk
    public AudioClip huskAlive;
    public AudioClip huskWalk;
    public AudioClip huskCry;
    public AudioClip huskDie;
    //Crawler
    public AudioClip crawlerWalk;
    public AudioClip crawlerCry;
    public AudioClip crawlerDie;

    private void Start()  
    {
        allAudio = GetComponents<AudioSource>();
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
            GunSource.PlayOneShot(shoot, 10f);
            player.GetComponent<WeaponSelect>().selectedWeapon.attack = false;
            //GetComponent<AudioManager>().SFXvolume
        }
    }

    public void playReloadingSound()
    {
        if (player.GetComponent<WeaponSelect>().selectedWeapon.reloadSFX == true)
        {
            GunSource.PlayOneShot(reload, 10f);
            player.GetComponent<WeaponSelect>().selectedWeapon.reloadSFX = false;
            //GetComponent<AudioManager>().SFXvolume
        }
    }
}
