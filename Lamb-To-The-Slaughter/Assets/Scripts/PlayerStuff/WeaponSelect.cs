using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeaponSelect : MonoBehaviour
{
    public enum Weapon
    {
        TestWeapon
    }

    public List<Weapon> StartingWeapons = new List<Weapon>();
    public Weapon startingSelectedWeapon;
    public Animator anim;
    public GameObject fireParticles;
    public Transform particlePos;
    public GunRecoil gunRecoil;
    public ParticleSystem reloadSmoke;
    public GameObject wallShot;

    [HideInInspector]
    public List<BaseWeapon> AvaliableWeapons = new List<BaseWeapon>();
    public BaseWeapon selectedWeapon;
    public CameraShake cameraShake;
    PlayerHealth ph;

    //Graphics
    float AOEforce;
    float AOEradius;
    public UnityEngine.Rendering.Universal.ChromaticAberration AOEcA;
    public UnityEngine.Rendering.Universal.Vignette AOEv;
    public UnityEngine.Rendering.VolumeProfile vp;
    float originalCA;
    public Color originalV;
    Color targetV;
    Color setV;
    public Gradient glassColour;
    public Material[] gunPower;

    //Sound
    public AudioSource audioSource;
    public AudioClip heal;

    [Header("Bomb Prefabs")]
    [SerializeField] Rigidbody gravityBomb;
    [SerializeField] Rigidbody gasBomb;
    [SerializeField] Rigidbody explosiveBomb;
    [SerializeField] Rigidbody teleportBomb;
    [SerializeField] Transform bombSpawner;
    [SerializeField] float horizontalVelocity;

    public GameObject player;


    private void Start()
    {
        CheckIfStartingWeaponsIsEmpty();
        AssignAvaliableWeapons();
        AssignStartingWeapon();
        FindPostProcessEffects();
        player = GameObject.FindGameObjectWithTag("Player");
        ph = player.GetComponent<PlayerHealth>();
        AOEv.color.Override(originalV);
        audioSource = GetComponent<AudioSource>();
    }

    //Instantiating
    private void CheckIfStartingWeaponsIsEmpty()
    {
        if (StartingWeapons.Count == 0)
        {
            Debug.LogError("No Starting Weapons Assigned");
        }
    }
    private void AssignAvaliableWeapons()
    {
        foreach (Weapon startingWeapon in StartingWeapons)
        {
            if (!InAvaliableWeapons(startingWeapon))
            {
                AvaliableWeapons.Add(EnumToWeapon(startingWeapon));
            }
        }
    }
    private void AssignStartingWeapon()
    {
        BaseWeapon weaponToStartWith = GetAvaliableWeapon(startingSelectedWeapon);
        if (weaponToStartWith != null)
            selectedWeapon = weaponToStartWith;
        else
            Debug.LogError("Starting Weapon has not been assigned");


    }

    private void SwitchWeapon(BaseWeapon WeaponToSwitchTo)
    {
        if (GetAvaliableWeapon(WeaponToSwitchTo.weaponAttributes.WeaponName) != null)
        {
            selectedWeapon = WeaponToSwitchTo;
        }
    }

    private void SwitchWeapon(Weapon weaponToSwitchTo)
    {
        BaseWeapon currentWeaponSwitch = GetAvaliableWeapon(weaponToSwitchTo);
        if (currentWeaponSwitch != null)
            selectedWeapon = currentWeaponSwitch;
        else
        {
            Debug.LogError("Weapon is not avaliable.");
        }
        selectedWeapon.ActivateWeaponHUD();
    }

    private BaseWeapon GetAvaliableWeapon(Weapon weaponToGet)
    {
        foreach (BaseWeapon weaponInAvaliableWeapon in AvaliableWeapons)
        {
            if (weaponInAvaliableWeapon.weaponAttributes.WeaponName == weaponToGet)
            {
                return weaponInAvaliableWeapon;
            }
        }
        return null;
    }

    private void Inputs()
    {
        if (Input.GetButton("Fire1") && selectedWeapon != null && selectedWeapon.current_ammo > 0 && !selectedWeapon.reloading && selectedWeapon.canShoot && PlayerHealth.overDrive)
        {
            selectedWeapon.Fire();

            Instantiate(fireParticles, particlePos.transform.position, particlePos.transform.rotation);
            fireParticles.transform.parent = particlePos.gameObject.transform;
            AOEv.color.Override(Color.white);
            AOEcA.intensity.Override(0.5f);
            StartCoroutine(cameraShake.Shake(0.25f, 4f));
            gunRecoil.StartRecoil();

            if (selectedWeapon.raycastHit.transform != null)
            {
                Vector3 wallNormal = (selectedWeapon.raycastHit.normal) * 90;
                Instantiate(wallShot, selectedWeapon.raycastHit.point, Quaternion.Euler(wallNormal.x, wallNormal.y + 90, wallNormal.z));
            }
        }
        if (Input.GetButtonDown("Fire1") && selectedWeapon != null && selectedWeapon.current_ammo > 0 && !selectedWeapon.reloading && selectedWeapon.canShoot)
        {
            selectedWeapon.Fire();

            Instantiate(fireParticles, particlePos.transform.position, particlePos.transform.rotation);
            fireParticles.transform.parent = particlePos.gameObject.transform;
            AOEv.color.Override(Color.white);
            AOEcA.intensity.Override(0.5f);
            StartCoroutine(cameraShake.Shake(0.25f, 4f));
            gunRecoil.StartRecoil();
            //StartCoroutine(gunRecoil.Recoil(0.05f, 0.3f));

            if (selectedWeapon.raycastHit.transform != null)
            {
                Vector3 wallNormal = (selectedWeapon.raycastHit.normal) * 90;
                Instantiate(wallShot, selectedWeapon.raycastHit.point, Quaternion.Euler(wallNormal.x, wallNormal.y + 90, wallNormal.z));
            }
        }

        if (Input.GetButton("AOE") && selectedWeapon != null)
        {
            Invoke("AOEattack", 0.3f);
        }
    }

    void AOEattack()
    {
        AOEradius = 15f;
        AOEforce = 50f;

        StartCoroutine(cameraShake.Shake(0.5f, 10f));
        AOEcA.intensity.Override(1f);
        AOEv.color.Override(Color.white);

        Collider[] nearbyRb = Physics.OverlapSphere(transform.position, AOEradius);
        foreach (Collider hit in nearbyRb)
        {

            Rigidbody forceRb = hit.GetComponent<Rigidbody>();
            if (forceRb != null)
            {
                forceRb.AddExplosionForce(AOEforce, transform.position, AOEradius, 1.0f, ForceMode.Impulse);
            }
        }
        Invoke("AOEgraphicsReset", 0.1f);
    }

    public void AOEgraphicsReset()
    {
        if (PlayerHealth.overDrive)
        {
            AOEv.color.Override(new Color(0.307f, 0.49f, 0.433f));
            AOEcA.intensity.Override(0.6f);
        }

        AOEcA.intensity.value = Mathf.Lerp(AOEcA.intensity.value, originalCA, 5f * Time.deltaTime);
        AOEv.color.value = Color.Lerp(AOEv.color.value, setV, 5f * Time.deltaTime);
    }

    private void Update()
    {
        Inputs();
        AOEgraphicsReset();
        AmmoGraphics();

        selectedWeapon.Update();

        if (selectedWeapon.current_ammo < 1)
        {
            reloadSmoke.Play();
        }

        //Tools
        if (!isBombThrowing())
        {
            StartCoroutine(GravityBomb());
            StartCoroutine(ExplosiveBomb());
            StartCoroutine(TeleportBomb());
            StartCoroutine(GasBomb());
        }
        MedPack();

        if (anim.GetBool("CanMelee") == true)
        {
            Invoke("ResetMelee", 0.2f);
        }
    }
    private bool InAvaliableWeapons(Weapon weapon)
    {
        foreach (BaseWeapon baseWeapon in AvaliableWeapons)
        {
            if (baseWeapon.weaponAttributes.WeaponName == weapon)
            {
                return true;
            }
        }
        return false;
    }
    public BaseWeapon EnumToWeapon(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.TestWeapon:
                return new TestWeapon(this);
        }
        return null;
    }

    public float meleeRange = 3f;

    public void MeleeAttack(RaycastHit hit)
    {
        anim.SetBool("CanMelee", true);
    }



    void MedPack()
    {
        if (Input.GetButtonDown("Medpack") && player.GetComponent<Inventory>().medpack >= 1)
        {
            audioSource.PlayOneShot(heal, 60f);
            player.GetComponent<Health>().currentHealth += 20;
            player.GetComponent<Inventory>().medpack--;
        }
    }

    public float timetowait;

    bool throwingBomb = false;

    public bool isBombThrowing()
    {
        bool gravityBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_B_40");
        bool gasBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_G_40");
        bool explosiveBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_E_40");
        bool teleportBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_T_40");
        return gravityBomb || gasBomb || explosiveBomb || teleportBomb;
    }

    IEnumerator GravityBomb()
    {
        if (player.GetComponent<Inventory>().gravityBomb >= 1 && Input.GetButtonDown("gravityBomb") && !throwingBomb && !isBombThrowing())
        {
            throwingBomb = true;
            //Instantiate Bomb Here
            anim.SetBool("GravityBomb", true);
            player.GetComponent<Inventory>().gravityBomb--;

            Debug.Log("instantiating");
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(gravityBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            throwingBomb = false;
            anim.SetBool("GravityBomb", false);

        }
    }

    IEnumerator ExplosiveBomb()
    {
        if (player.GetComponent<Inventory>().explosionBomb >= 1 && Input.GetButtonDown("explosionBomb") && !throwingBomb && !isBombThrowing())
        {
            throwingBomb = true;
            //instantiateBombs
            anim.SetBool("ExplosiveBomb", true);
            player.GetComponent<Inventory>().explosionBomb--;
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(explosiveBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            throwingBomb = false;
            anim.SetBool("ExplosiveBomb", false);
        }
    }

    IEnumerator TeleportBomb()
    {
        if (player.GetComponent<Inventory>().teleportBomb >= 1 && Input.GetButtonDown("teleportBomb") && !throwingBomb && !isBombThrowing())
        {
            throwingBomb = true;
            //instantiateBomb
            anim.SetBool("TeleportBomb", true);
            player.GetComponent<Inventory>().teleportBomb--;
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(teleportBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            throwingBomb = false;
            anim.SetBool("TeleportBomb", false);
        }
    }

    IEnumerator GasBomb()
    {
        if (player.GetComponent<Inventory>().gasBomb >= 1 && Input.GetButtonDown("gasBomb") && !throwingBomb && !isBombThrowing())
        {
            throwingBomb = true;
            //instantiateBomb
            anim.SetBool("GasBomb", true);
            player.GetComponent<Inventory>().gasBomb--;
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(gasBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            throwingBomb = false;
        }
        anim.SetBool("GasBomb", false);
    }

    void ResetMelee()
    {
        anim.SetBool("CanMelee", false);
    }

    void FindPostProcessEffects()
    {
        ChromaticAberration cA;
        Vignette v;

        if (vp.TryGet<ChromaticAberration>(out cA))
        {
            AOEcA = cA;
        }

        if (vp.TryGet<Vignette>(out v))
        {
            AOEv = v;
        }

        originalCA = 0.161f;
        originalV = new Color(0.207f, 0.032f, 0.032f);
        setV = (originalV + targetV) / 2;

    }

    public void StartWeaponCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

    void AmmoGraphics()
    {
        float currentAmmo = selectedWeapon.current_ammo;

        float scaledValue = currentAmmo / 10;

        foreach (Material mat in gunPower)
        {
            mat.SetColor("_EmissionColor", glassColour.Evaluate(scaledValue));
        }
    }
}
