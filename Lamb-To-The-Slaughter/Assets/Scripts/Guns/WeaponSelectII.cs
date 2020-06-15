using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelectII : MonoBehaviour
{
    public enum Weapon
    {
        Exorcist
    }

    public List<Weapon> StartingWeapons = new List<Weapon>();
    public Weapon startingSelectedWeapon;

    [HideInInspector]
    public List<BaseWeaponII> AvaliableWeapons = new List<BaseWeaponII>();
    public BaseWeaponII selectedWeapon;

    public Animator animator;
    public Transform particleSpawner;


    private void Start()
    {
        CheckIfStartingWeaponsIsEmpty();
        AssignAvaliableWeapons();
        AssignStartingWeapon();
        selectedWeapon.Initialize();
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
        BaseWeaponII weaponToStartWith = GetAvaliableWeapon(startingSelectedWeapon);
        if (weaponToStartWith != null)
            selectedWeapon = weaponToStartWith;
        else
            Debug.LogError("Starting Weapon has not been assigned");


    }

    private void SwitchWeapon(BaseWeaponII WeaponToSwitchTo)
    {
        if (GetAvaliableWeapon(WeaponToSwitchTo.weaponStats.weapon) != null)
        {
            selectedWeapon = WeaponToSwitchTo;
        }
    }

    private void SwitchWeapon(Weapon weaponToSwitchTo)
    {
        BaseWeaponII currentWeaponSwitch = GetAvaliableWeapon(weaponToSwitchTo);
        if (currentWeaponSwitch != null)
            selectedWeapon = currentWeaponSwitch;
        else
        {
            Debug.LogError("Weapon is not avaliable.");
        }
        //selectedWeapon.ActivateWeaponHUD();
    }

    private BaseWeaponII GetAvaliableWeapon(Weapon weaponToGet)
    {
        foreach (BaseWeaponII weaponInAvaliableWeapon in AvaliableWeapons)
        {
            if (weaponInAvaliableWeapon.weaponStats.weapon == weaponToGet)
            {
                return weaponInAvaliableWeapon;
            }
        }
        return null;
    }

    private void Inputs()
    {

    }

    private void Update()
    {
        Inputs();
    }
    private bool InAvaliableWeapons(Weapon weapon)
    {
        foreach (BaseWeaponII baseWeaponII in AvaliableWeapons)
        {
            if (baseWeaponII.weaponStats.weapon == weapon)
            {
                return true;
            }
        }
        return false;
    }
    public BaseWeaponII EnumToWeapon(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.Exorcist:
                return new Exorcist(this);
        }
        return null;
    }

}
