﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWeapon //NEEDS COMMENTING
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

    public RaycastHit ShootRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out hit, weaponAttributes.Range, ignoreLayer, QueryTriggerInteraction.Ignore);
        return hit;
    }

    public RaycastHit ShootRaycastWithoutRange()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
            out hit, Mathf.Infinity, ignoreLayer, QueryTriggerInteraction.Ignore);
        return hit;
    }

    public BaseWeapon(IWeaponAttributes weaponAttributes, WeaponSelect weaponSelect)
    {
        this.weaponSelect = weaponSelect;
        current_ammo = weaponAttributes.Ammo;
        ignoreLayer = (1 << 10);
        ignoreLayer = ~ignoreLayer;
        this.weaponAttributes = weaponAttributes;
    }

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

        raycastHit = ShootRaycast();



    }

    public float weaponDelayOverdrive = 0.1f;

    public IEnumerator OverdriveWeaponDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponDelayOverdrive);
        canShoot = true;
    }

    public IEnumerator WeaponDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(weaponAttributes.WeaponDelay);
        canShoot = true;
    }

    public virtual void Update()
    {
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
    public void Fire2()
    {
        if (aiming)
            aiming = false;
        else
        {
            aiming = false;
        }
    }

    public bool isAiming()
    {
        return aiming;
    }

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

    public void ActivateWeaponHUD()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("WeaponHUD"));
        GameObject.Instantiate(weaponAttributes.weaponHUD);
    }

}
