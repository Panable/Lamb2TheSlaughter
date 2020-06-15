using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Exorcist : BaseWeaponII
{

    public Exorcist(WeaponSelectII weaponSelect) : base(weaponSelect)
    {
        weaponStats = Resources.Load<WeaponStats>("GunStats/Exorcist");
    }

    public virtual void Initialize()
    {
       
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

}
