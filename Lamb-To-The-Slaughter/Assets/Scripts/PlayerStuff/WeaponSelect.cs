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
    bool smoothSet;


    [HideInInspector]
    public List<BaseWeapon> AvaliableWeapons = new List<BaseWeapon>();
    public BaseWeapon selectedWeapon;
    int ammoCount = 5;
    public CameraShake cameraShake;

    //AOE
    float AOEforce;
    float AOEradius;
    public UnityEngine.Rendering.Universal.ChromaticAberration AOEcA;
    public UnityEngine.Rendering.Universal.LensDistortion AOElD;
    public UnityEngine.Rendering.Universal.Vignette AOEv;
    public UnityEngine.Rendering.VolumeProfile vp;
    float originalCA;
    float originalLD;
    Color originalV;
    float currentCAvalue;


    private void Start()
    {
        CheckIfStartingWeaponsIsEmpty();
        AssignAvaliableWeapons();
        AssignStartingWeapon();
        FindPostProcessEffects();
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
            AOEcA.intensity.Override(1f);
            StartCoroutine(cameraShake.Shake(0.25f, 4f));
            StartCoroutine(gunRecoil.Recoil(0.05f, 0.3f));
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

        StartCoroutine(cameraShake.Shake(0.25f, 5f));
        AOEcA.intensity.Override(1f);
        AOElD.intensity.Override(0.3f);
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

    void AOEgraphicsReset()
    {
        AOEcA.intensity.value = Mathf.Lerp(AOEcA.intensity.value, originalCA, 5f * Time.deltaTime);
        AOElD.intensity.value = Mathf.Lerp(AOElD.intensity.value, originalLD, 5f * Time.deltaTime);
        AOEv.color.value = Color.Lerp(AOEv.color.value, originalV, 5f * Time.deltaTime);
        //AOEv.color.Override(originalV);
    }

    private void Update()
    {
        Inputs();
        AOEgraphicsReset();

        selectedWeapon.Update();

        GravityBomb();
        ExplosiveBomb();
        TeleportBomb();
        GasBomb();

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

    void GravityBomb()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //InstantiateBomb
            anim.SetBool("GravityBomb", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            anim.SetBool("GravityBomb", false);
        }
    }

    void ExplosiveBomb()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //instantiateBomb
            anim.SetBool("ExplosiveBomb", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            anim.SetBool("ExplosiveBomb", false);
        }
    }

    void TeleportBomb()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //instantiateBomb
            anim.SetBool("TeleportBomb", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            anim.SetBool("TeleportBomb", false);
        }
    }

    void GasBomb()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //instantiateBomb
            anim.SetBool("GasBomb", true);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
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
        LensDistortion lD;
        Vignette v;

        if (vp.TryGet<ChromaticAberration>(out cA))
        {
            AOEcA = cA;
        }

        if (vp.TryGet<LensDistortion>(out lD))
        {
            AOElD = lD;
        }

        if (vp.TryGet<Vignette>(out v))
        {
            AOEv = v;
        }

        originalCA = AOEcA.intensity.value;
        originalLD = AOElD.intensity.value;
        originalV = AOEv.color.value;
        Debug.Log(originalV);
    }

    public void StartWeaponCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

}
