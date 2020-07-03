using UnityEngine;

public class AudioPlayer : MonoBehaviour //Lachlan
{
    //Audio Sources
    public GameObject player;
    public AudioSource audioSource;

    //Gun Sounds
    public AudioClip shoot;
    public AudioClip reload;
    public AudioClip AOE;
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
        AOEAttackSound();
    }

    public void AOEAttackSound()
    {
        if (player.GetComponent<WeaponSelect>().AOEsoundplay == true)
        {
            audioSource.PlayOneShot(AOE, 1f);
            player.GetComponent<WeaponSelect>().AOEsoundplay = false;
        }
    }

    public void playShootSound()
    {
        if (player.GetComponent<WeaponSelect>().selectedWeapon.attack == true)
        {
            audioSource.PlayOneShot(shoot, 2f);
            player.GetComponent<WeaponSelect>().selectedWeapon.attack = false;
        }
    }

    public void playReloadingSound()
    {
        if (player.GetComponent<WeaponSelect>().selectedWeapon.reloadSFX == true)
        {
            audioSource.PlayOneShot(reload, 1f);
            player.GetComponent<WeaponSelect>().selectedWeapon.reloadSFX = false;
        }
    }

}
