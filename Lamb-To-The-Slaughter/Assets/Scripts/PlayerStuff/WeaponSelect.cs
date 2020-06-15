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
    int ammoCount = 5;
    public CameraShake cameraShake;

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

    public GameObject player;


    private void Start()
    {
        CheckIfStartingWeaponsIsEmpty();
        AssignAvaliableWeapons();
        AssignStartingWeapon();
        FindPostProcessEffects();
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (Input.GetButtonDown("Fire1") && selectedWeapon != null && selectedWeapon.current_ammo > 0 && !selectedWeapon.reloading && selectedWeapon.canShoot)
        {
            selectedWeapon.Fire();

            Instantiate(fireParticles, particlePos.transform.position, particlePos.transform.rotation);
            fireParticles.transform.parent = particlePos.gameObject.transform;
            AOEv.color.Override(Color.white);
            AOEcA.intensity.Override(0.5f);
            StartCoroutine(cameraShake.Shake(0.25f, 4f));
            StartCoroutine(gunRecoil.Recoil(0.05f, 0.3f));

            if (selectedWeapon.raycastHit.transform != null)
            {
                Vector3 wallNormal = (selectedWeapon.raycastHit.normal) * 90;
                Instantiate(wallShot, selectedWeapon.raycastHit.point, Quaternion.Euler(wallNormal.x, wallNormal.y + 90, wallNormal.z));
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedWeapon != null)
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
        AOEcA.intensity.value = Mathf.Lerp(AOEcA.intensity.value, originalCA, 5f * Time.deltaTime);
        AOEv.color.value = Color.Lerp(AOEv.color.value, setV, 5f * Time.deltaTime);
    }

    private void Update()
    {
        Inputs();
        AOEgraphicsReset();

        selectedWeapon.Update();

        if (selectedWeapon.current_ammo < 1)
        {
            reloadSmoke.Play();
        }

        //Tools
        GravityBomb();
        ExplosiveBomb();
        TeleportBomb();
        GasBomb();
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
        if (Input.GetKeyDown(KeyCode.Alpha1) && player.GetComponent<Inventory>().medpack >= 1)
        {
                //Medpack Animation or visual indicator here if that's a thing.
                player.GetComponent<Health>().currentHealth += 20;
                player.GetComponent<Inventory>().medpack--;
        }
    }

    void GravityBomb()
    {
        if (player.GetComponent<Inventory>().gravityBomb >= 1 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Instantiate Bomb Here
            anim.SetBool("GravityBomb", true);
            player.GetComponent<Inventory>().gravityBomb--;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            anim.SetBool("GravityBomb", false);
        }
    }

    void ExplosiveBomb()
    {
        if (player.GetComponent<Inventory>().explosionBomb >=1 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            //instantiateBombs
            anim.SetBool("ExplosiveBomb", true);
            player.GetComponent<Inventory>().explosionBomb--;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            anim.SetBool("ExplosiveBomb", false);
        }
    }

    void TeleportBomb()
    {
        if (player.GetComponent<Inventory>().teleportBomb >= 1 && Input.GetKeyDown(KeyCode.Alpha4))
        {
            //instantiateBomb
            anim.SetBool("TeleportBomb", true);
            player.GetComponent<Inventory>().teleportBomb--;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            anim.SetBool("TeleportBomb", false);
        }
    }

    void GasBomb()
    {
        if (player.GetComponent<Inventory>().gasBomb >= 1 && Input.GetKeyDown(KeyCode.Alpha5))
        {
            //instantiateBomb
            anim.SetBool("GasBomb", true);
            player.GetComponent<Inventory>().gasBomb--;
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            anim.SetBool("GasBomb", false);
        }
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
        originalV = AOEv.color.value;
        setV = (originalV + targetV) / 2;
        
    }

    public void StartWeaponCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

}
