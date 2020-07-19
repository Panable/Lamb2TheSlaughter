using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Audio;

public class WeaponSelect : MonoBehaviour //Dhan
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
    public GameObject pauseMenu;
    public LayerMask ignore;

    [HideInInspector]
    public List<BaseWeapon> AvaliableWeapons = new List<BaseWeapon>();
    public BaseWeapon selectedWeapon;
    public CameraShake cameraShake;
    PlayerHealth ph;

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
    public Gradient glassColour;
    public Material[] gunPower;
    public Material bubbleShader;
    public GameObject damageParticles;
    public GameObject shockwave;
    public Transform shockwaveAnchor;
    public GameObject bombUI;

    //Audio
    public AudioSource audioSource;
    public AudioClip heal;
    public bool AOEsoundplay;

    public AudioSource audioSourceThrow;
    public AudioClip[] bombThrowAudio;

    [Header("Bomb Prefabs")]
    [SerializeField] Rigidbody gravityBomb;
    [SerializeField] Rigidbody gasBomb;
    [SerializeField] Rigidbody explosiveBomb;
    [SerializeField] Rigidbody teleportBomb;
    [SerializeField] Transform bombSpawner;
    [SerializeField] float horizontalVelocity;

    public GameObject player;
    public ToolSetButton medpackButton;
    public ToolSetButton gasButton;
    public ToolSetButton gravityButton;
    public ToolSetButton teleportButton;
    public ToolSetButton explosiveButton;
    ToolManager tm;


    private void Start()
    {
        CheckIfStartingWeaponsIsEmpty();
        AssignAvaliableWeapons();
        AssignStartingWeapon();
        FindPostProcessEffects();
        player = GameObject.FindGameObjectWithTag("Player");
        ph = player.GetComponent<PlayerHealth>();
        AOEv.color.Override(originalV);
        audioSourceThrow.loop = false;
        tm = medpackButton.GetComponentInParent<ToolManager>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region Initializing
    //error checking
    private void CheckIfStartingWeaponsIsEmpty()
    {
        if (StartingWeapons.Count == 0)
        {
            Debug.LogError("No Starting Weapons Assigned");
        }
    } 
    //Instantiate starting avaliable weapon list
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
    //assign starting weapon list
    private void AssignStartingWeapon()
    {
        BaseWeapon weaponToStartWith = GetAvaliableWeapon(startingSelectedWeapon);
        if (weaponToStartWith != null)
            selectedWeapon = weaponToStartWith;
        else
            Debug.LogError("Starting Weapon has not been assigned");
    }
    #endregion

    //weapon switching
    private void SwitchWeapon(BaseWeapon WeaponToSwitchTo)
    {
        if (GetAvaliableWeapon(WeaponToSwitchTo.weaponAttributes.WeaponName) != null)
        {
            selectedWeapon = WeaponToSwitchTo;
        }
    }

    //weapon switching
    private void SwitchWeapon(Weapon weaponToSwitchTo)
    {
        BaseWeapon currentWeaponSwitch = GetAvaliableWeapon(weaponToSwitchTo);
        if (currentWeaponSwitch != null)
            selectedWeapon = currentWeaponSwitch;
        else
        {
            Debug.LogError("Weapon is not avaliable.");
        }
    }

    //returns an avaliable weapon (null if no weapon is found)
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

    //Managing input
    private void Inputs()
    {
        //Overdrive fire
        if (Input.GetButton("Fire1") && selectedWeapon != null && selectedWeapon.current_ammo > 0 && !selectedWeapon.reloading && selectedWeapon.canShoot && PlayerHealth.overDrive && !toolsetControl)
        {
            selectedWeapon.Fire();

            //Animations for weapon fire
            Instantiate(fireParticles, particlePos.transform.position, particlePos.transform.rotation);
            AOEv.color.Override(Color.white);
            AOEcA.intensity.Override(0.5f);
            StartCoroutine(cameraShake.Shake(0.15f, 3f));
            gunRecoil.StartRecoil();

            //particles for weapon fire
            if (selectedWeapon.raycastHit.transform != null)
            {
                Vector3 wallNormal = (selectedWeapon.raycastHit.normal) * 90;
                Instantiate(wallShot, selectedWeapon.raycastHit.point, Quaternion.Euler(wallNormal.x, wallNormal.y + 90, wallNormal.z));
            }
        }
        //Default fire
        if (Input.GetButtonDown("Fire1") && selectedWeapon != null && selectedWeapon.current_ammo > 0 && !selectedWeapon.reloading && selectedWeapon.canShoot && !toolsetControl)
        {
            selectedWeapon.Fire();

            //animations
            Instantiate(fireParticles, particlePos.transform.position, particlePos.transform.rotation);
            AOEv.color.Override(Color.white);
            AOEcA.intensity.Override(0.5f);
            StartCoroutine(cameraShake.Shake(0.15f, 3f));
            gunRecoil.StartRecoil();

            //particles
            if (selectedWeapon.raycastHit.transform != null)
            {
                Vector3 wallNormal = (selectedWeapon.raycastHit.normal) * 90;
                if (selectedWeapon.raycastHit.transform.CompareTag("Enemy"))
                {
                    Instantiate(damageParticles, selectedWeapon.raycastHit.point, Quaternion.Euler(wallNormal.x, wallNormal.y + 90, wallNormal.z));
                }
                else
                {
                    Instantiate(wallShot, selectedWeapon.raycastHit.point, Quaternion.Euler(wallNormal.x, wallNormal.y + 90, wallNormal.z));

                }
            }
        }

        if (Input.GetButton("AOE") && selectedWeapon != null && canAOE)
        {
            AOEattack();
        }
    }

    bool canAOE = true;
    public float aoeCooldown = 4f;
    public bool hasScreamed;

    void AOEattack()
    {
        canAOE = false;
        AOEradius = 15f;
        AOEforce = 50f;

        StartCoroutine(cameraShake.Shake(0.3f, 5f));
        AOEsoundplay = true;
        AOEcA.intensity.Override(0.4f);
        AOEv.color.Override(Color.white);
        float randRot = UnityEngine.Random.Range(0f, 90f);
        Instantiate(shockwave, shockwaveAnchor.position, Quaternion.Euler(270f, randRot, 0));
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

        aoeCooldown = 0f;
        hasScreamed = true;

        //Invoke("AOECooldown", aoeCooldown);
    }

    void AOECooldown()
    {
        canAOE = true;
    }

    public void AOEgraphicsReset()
    {
        if (PlayerHealth.overDrive)
        {
            AOEv.color.Override(new Color(0.307f, 0.49f, 0.433f));
            AOEcA.intensity.Override(0.2f);
        }
        AOEcA.intensity.value = Mathf.Lerp(AOEcA.intensity.value, originalCA, 5f * Time.deltaTime);
        AOEv.color.value = Color.Lerp(AOEv.color.value, setV, 5f * Time.deltaTime);
    }

    private void Update()
    {
        Inputs();
        AOEgraphicsReset();
        AmmoGraphics();
        BombThrow();

        aoeCooldown = Mathf.Clamp(aoeCooldown, 0, 4);

        if (hasScreamed)
        {
            aoeCooldown = aoeCooldown + Time.deltaTime;
            if (aoeCooldown > 4)
            {
                aoeCooldown = 4;
                canAOE = true;
                hasScreamed = false;
            }
        }

        selectedWeapon.Update();

        if (selectedWeapon.current_ammo < 1)
        {
            reloadSmoke.Play();
        }

        //Tools
        if (!isBombThrowing())
        {
            StartCoroutine(GravityBomb());
            StartCoroutine(ExplosiveBomb());
            StartCoroutine(TeleportBomb());
            StartCoroutine(GasBomb());
        }
        MedPack();

        if (anim.GetBool("CanMelee") == true)
        {
            Invoke("ResetMelee", 0.2f);
        }
    }
    
    //check if a weapon is available 
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
    //convert from enum to weapon obj
    public BaseWeapon EnumToWeapon(Weapon weapon)
    {
        switch (weapon)
        {
            case Weapon.TestWeapon:
                return new TestWeapon(this);
        }
        return null;
    }


    void MedPack()
    {
        if (medpackButton.clicked && GetComponent<Inventory>().medpack >= 1)
        {
            audioSource.PlayOneShot(heal, 30f);
            GetComponent<Health>().currentHealth += 10;
            GetComponent<Inventory>().medpack--;
            Cursor.lockState = CursorLockMode.Locked;
            medpackButton.clicked = false;
        }
        else if (medpackButton.clicked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public float timetowait;

    bool throwingBomb = false;

    //Bomb methods for throwing and input
    #region bombs
    public bool isBombThrowing()
    {
        player.GetComponent<WeaponSelect>().AOEsoundplay = false;
        bool gravityBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_B_40");
        bool gasBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_G_40");
        bool explosiveBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_E_40");
        bool teleportBomb = anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerRig|Gun_ThrowBomb_T_40");
        return gravityBomb || gasBomb || explosiveBomb || teleportBomb;
    }

    public IEnumerator GravityBomb()
    {
        if (GetComponent<Inventory>().gravityBomb >= 1 && gravityButton.clicked && !throwingBomb && !isBombThrowing())
        {
            throwingBomb = true;
            //Instantiate Bomb Here
            audioSourceThrow.loop = false;
            audioSourceThrow.clip = bombThrowAudio[UnityEngine.Random.Range(0, bombThrowAudio.Length)];
            audioSourceThrow.Play();
            anim.SetBool("GravityBomb", true);
            GetComponent<Inventory>().gravityBomb--;
            Debug.Log("instantiating");
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(gravityBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            Cursor.lockState = CursorLockMode.Locked;
            gravityButton.clicked = false;
            throwingBomb = false;
            anim.SetBool("GravityBomb", false);
        }
    }

    IEnumerator ExplosiveBomb()
    {
        if (GetComponent<Inventory>().explosionBomb >= 1 && explosiveButton.clicked && !throwingBomb && !isBombThrowing())
        {
            throwingBomb = true;
            //instantiateBombs
            audioSourceThrow.loop = false;
            audioSourceThrow.clip = bombThrowAudio[UnityEngine.Random.Range(0, bombThrowAudio.Length)];
            audioSourceThrow.Play();
            anim.SetBool("ExplosiveBomb", true);
            GetComponent<Inventory>().explosionBomb--;
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(explosiveBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            Cursor.lockState = CursorLockMode.Locked;
            explosiveButton.clicked = false;
            throwingBomb = false;
            anim.SetBool("ExplosiveBomb", false);
        }
    }

    IEnumerator TeleportBomb()
    {
        if (GetComponent<Inventory>().teleportBomb >= 1 && teleportButton.clicked && !throwingBomb && !isBombThrowing())
        {
            if (BombScript.teleport != null)
            {
                BombScript.teleport.transform.GetChild(0).gameObject.SetActive(false);
                BombScript.teleport.GetComponent<Rigidbody>().isKinematic = false;
                BombScript.teleport.GetComponent<Rigidbody>().useGravity = true;
                BombScript.teleport.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
                Destroy(BombScript.teleport.GetComponent<BombScript>());
            }
            throwingBomb = true;
            //instantiateBomb
            audioSourceThrow.loop = false;
            audioSourceThrow.clip = bombThrowAudio[UnityEngine.Random.Range(0, bombThrowAudio.Length)];
            audioSourceThrow.Play();
            anim.SetBool("TeleportBomb", true);
            GetComponent<Inventory>().teleportBomb--;
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(teleportBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            Cursor.lockState = CursorLockMode.Locked;
            teleportButton.clicked = false;
            throwingBomb = false;
            anim.SetBool("TeleportBomb", false);
        }
    }

    IEnumerator GasBomb()
    {
        if (GetComponent<Inventory>().gasBomb >= 1 && gasButton.clicked && !throwingBomb && !isBombThrowing())
        {
            throwingBomb = true;
            //instantiateBomb
            audioSourceThrow.loop = false;
            audioSourceThrow.clip = bombThrowAudio[UnityEngine.Random.Range(0, bombThrowAudio.Length)];
            audioSourceThrow.Play();
            anim.SetBool("GasBomb", true);
            GetComponent<Inventory>().gasBomb--;
            yield return new WaitForSeconds(timetowait);
            Rigidbody rb = Instantiate<Rigidbody>(gasBomb, bombSpawner.transform.position, Quaternion.identity);
            Vector3 location = selectedWeapon.ShootRaycastWithoutRange().point;
            rb.velocity = ((location - rb.transform.position).normalized * horizontalVelocity);
            Cursor.lockState = CursorLockMode.Locked;
            gasButton.clicked = false;
            throwingBomb = false;
            anim.SetBool("GasBomb", false);
        }
    }
    #endregion
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

        originalCA = 0.05f;
        originalV = new Color(0.207f, 0.032f, 0.032f);
        setV = (originalV + targetV) / 2;

    }

    public void StartWeaponCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }

    void AmmoGraphics()
    {
        float currentAmmo = selectedWeapon.current_ammo;

        float scaledValue = currentAmmo / 10;

        bubbleShader.SetColor("Color_29025F83", glassColour.Evaluate(scaledValue));

        foreach (Material mat in gunPower)
        {
            mat.SetColor("_EmissionColor", glassColour.Evaluate(scaledValue));
        }
    }

    public bool toolsetControl = false;
    bool cursorLocked = true;

    void BombThrow()
    {
        if (!pauseMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                cursorLocked = false;
                bombUI.SetActive(true);
                toolsetControl = true;
                Time.timeScale = 0.25f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) && !throwingBomb)
            {
                Cursor.lockState = CursorLockMode.Locked;
                toolsetControl = false;
            }

            if (!toolsetControl)
            {
                TextCorrectionForToolset(medpackButton);
                TextCorrectionForToolset(gasButton);
                TextCorrectionForToolset(gravityButton);
                TextCorrectionForToolset(teleportButton);
                TextCorrectionForToolset(explosiveButton);
                tm.activeButtons.Clear();

                bombUI.SetActive(false);
                Time.timeScale = 1f;
                Cursor.visible = false;
            }
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    void TextCorrectionForToolset(ToolSetButton button)
    {
        button.highlight = false;
        button.buttonActive = false;
        button.clicked = false;
        button.centreText.SetText("Tools");
        button.centreText.color = Color.white;
        button.centreText.fontSize = 36f;
    }
}
