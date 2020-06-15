using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeaponII
{
    public WeaponStats weaponStats;
    protected WeaponSelectII weaponSelect;
    public Transform weaponObject;
    public Transform particleSpawner;
    public Transform gunMesh;


    public void Initialize()
    {
        if (Camera.main.transform.childCount > 0)
        {
            foreach (Transform child in Camera.main.transform)
            {
                GameObject.Destroy(child);
            }
        }
        weaponObject = GameObject.Instantiate(weaponStats.weaponArmPrefab, Camera.main.transform);
        weaponObject.localPosition = weaponStats.hudLocation;
        gunMesh = ExtensionMethods.FindDeepChild(weaponObject, "GunMesh");
        particleSpawner = ExtensionMethods.FindDeepChild(weaponObject, "ParticleSpawn");
    }

    public BaseWeaponII(WeaponSelectII weaponSelect)
    {
        this.weaponSelect = weaponSelect;
    }

    public virtual void Fire()
    {

    }

    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {

    }


}
