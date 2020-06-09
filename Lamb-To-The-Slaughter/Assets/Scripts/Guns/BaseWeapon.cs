using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseWeapon
{
    protected LayerMask ignoreLayer;
    public bool reloading = false;
    public int current_ammo;
    public bool aiming = false;

    public IWeaponAttributes weaponAttributes;
    public interface IWeaponAttributes
    {
        float Damage { get; }
        float Range { get; }
        int Ammo { get; }
        WeaponSelect.Weapon WeaponName { get; }
        GameObject WeaponModel { get; }
        Canvas weaponHUD { get; }

    }

    public BaseWeapon(IWeaponAttributes weaponAttributes)
    {
        ignoreLayer = LayerMask.NameToLayer("ignore");
        Debug.Log(ignoreLayer);
        this.weaponAttributes = weaponAttributes;
    }

    public abstract void Fire();
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

    public void Reload()
    {
        reloading = true;


        // do animation / timer here w/e
        current_ammo = weaponAttributes.Ammo;

        reloading = false;

    }

    public void ActivateWeaponHUD()
    {
        GameObject.Destroy(GameObject.FindGameObjectWithTag("WeaponHUD"));
        GameObject.Instantiate(weaponAttributes.weaponHUD);
    }

}
