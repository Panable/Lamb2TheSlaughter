using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponStats")]
public class WeaponStats : ScriptableObject
{

    public float damage;
    public float range;
    public int ammo;
    public float weaponDelay;
    public WeaponSelectII.Weapon weapon;

    public GameObject fireParticles;

    public GunRecoil gunRecoil;

    public GameObject wallShot;

    public Transform weaponArmPrefab;

    public Vector3 hudLocation;
    public AudioClip reloadSound;

}
