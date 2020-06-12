using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : BaseWeapon
{
    WeaponSelect weaponSelect;
    public static GameObject player;
    public struct WeaponAttributes : IWeaponAttributes
    {
        public static float Base_Damage = 1;
        public static float Base_Range = 20;
        public static int Base_Ammo = 10;
        public static float Weapon_Delay = 0.4f;
        public  bool shot;

        public float Damage
        {
            get
            {
                return Base_Damage;
            }
        }

        public int Ammo
        {
            get
            {
                return Base_Ammo;
            }
        }
        public float WeaponDelay
        {
            get
            {
                return Weapon_Delay;
            }
        }

        public float Range
        {
            get
            {
                return Base_Range;
            }
        }

        public WeaponSelect.Weapon WeaponName
        {
            get
            {
                return WeaponSelect.Weapon.TestWeapon;
            }
        }
        public GameObject WeaponModel
        {
            get
            {
                return null;
            }
        }
        public Canvas weaponHUD
        {
            get
            {
                return Resources.Load<Canvas>("Crosshairs/Psych_Crosshair");
            }
        }

    }
    public static Transform target;

    public TestWeapon(WeaponSelect weaponSelect) : base(new WeaponAttributes(), weaponSelect)
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public float meleeRange = 3f;

    public void MeleeAttack(RaycastHit hit)
    {
        //meleeAttack;
    }

    public override void Fire()
    {
        bool attack;
        base.Fire();
        attack = true;
        //Debug.Log("tryfire");
        attack = false;

        if (raycastHit.transform != null)
        {
            //Debug.Log(hit.transform.name);
            if (raycastHit.transform.tag == "Enemy")
            {
                //Debug.Log(raycastHit.transform.name);
                if (raycastHit.distance <= weaponSelect.meleeRange)
                {
                    weaponSelect.MeleeAttack(raycastHit);
                    raycastHit.transform.GetComponent<Health>().TakeDamage(WeaponAttributes.Base_Damage);
                }
                else
                {
                    raycastHit.transform.GetComponent<Health>().TakeDamage(WeaponAttributes.Base_Damage);
                }
            }
        }

        if (current_ammo <= 0)
        {
            Reload();
        }

    }


    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }
}
