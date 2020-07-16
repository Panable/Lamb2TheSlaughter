using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWeapon //Dhan
{
    public RaycastHit raycastHit;
    public bool canShoot = true;
    protected int ignoreLayer;
    public bool reloading = false;
    public int current_ammo;
    public bool aiming = false;
    WeaponSelect weaponSelect;

    //SFX
    public bool attack = false;
    public bool reloadSFX = false;

    public IWeaponAttributes weaponAttributes;

    //includes range dmg, etc
    public interface IWeaponAttributes
    {
        float Damage { get; }
        float Range { get; }
        int Ammo { get; }
        float WeaponDelay { get; }
        WeaponSelect.Weapon WeaponName { get; }
        GameObject WeaponModel { get; }
        Canvas weaponHUD { get; }

    }

    //raycast shooting used to determine what the player is looking at (with range)
    public RaycastHit ShootRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out hit, weaponAttributes.Range, ignoreLayer, QueryTriggerInteraction.Ignore);
        return hit;
    }

    //raycast shooting used to determine what the player is looking at (no range)
    public RaycastHit ShootRaycastWithoutRange()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out hit, Mathf.Infinity, ignoreLayer, QueryTriggerInteraction.Ignore);
        return hit;
    }

    //Initialization
    public BaseWeapon(IWeaponAttributes weaponAttributes, WeaponSelect weaponSelect)
    {
        this.weaponSelect = weaponSelect;
        current_ammo = weaponAttributes.Ammo;
        ignoreLayer = (1 << 10);
        ignoreLayer = ~ignoreLayer;
        this.weaponAttributes = weaponAttributes;
    }

    //manages shooting weapon
    public virtual void Fire()
    {
        if (reloading || current_ammo <= 0)
            return;
        else
        {
            attack = true;
            current_ammo--;
            if (PlayerHealth.overDrive)
                weaponSelect.StartCoroutine(OverdriveWeaponDelay());
            else
                weaponSelect.StartCoroutine(WeaponDelay());
        }

        //initializes raycast
        raycastHit = ShootRaycast();



    }

    public float weaponDelayOverdrive = 0.1f;

    //weapon delay for overdrive mode
    public IEnumerator OverdriveWeaponDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponDelayOverdrive);
        canShoot = true;
    }

    //weapon delay for default mode
    public IEnumerator WeaponDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponAttributes.WeaponDelay);
        canShoot = true;
    }

    public virtual void Update()
    {
        //assign reloading attributes
        weaponSelect.anim.SetInteger("Reload", current_ammo);
        //Debug.Log("Current ammo = " + current_ammo);
        if (current_ammo <= 0 && !reloading)
        {
            weaponSelect.StartCoroutine(Reload());
        }

        if (Input.GetButtonDown("Reload") && current_ammo < weaponAttributes.Ammo && !reloading && !Input.GetButton("Fire1"))
        {
            current_ammo = 0;
            //Debug.Log("tryreload");
            weaponSelect.StartCoroutine(Reload());
        }

    }

    //reloads weapon
    public IEnumerator Reload()
    {
        reloading = true;
        reloadSFX = true;
        Animator anim = weaponSelect.anim;
        //Debug.Log("Waiting 1s " + weaponAttributes.Ammo);
        yield return new WaitForSeconds(1.1f);
        current_ammo = weaponAttributes.Ammo;
        reloading = false;
    }

}
