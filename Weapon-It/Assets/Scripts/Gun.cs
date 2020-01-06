using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Gun Parameters")]
    public float damage = 1;
    [Range(60, 180)]
    public float range = 60;
    [Range(.1f, 6)]
    public float fireRate;

    [HideInInspector]
    public int damageUpgradeCount, rangeUpgradeCount, fireRateUpgradeCount;

    [HideInInspector]
    public float recoilLerpMultiplier = 8;
    public float recoilForce = 3;

    [Header("Gun Objects - Editor")]
    public Transform slide;
    Vector3 slideOrigPos;

    public Transform muzzle;
    public Transform shellExit;

    // The gun location as a child of the weapons parent (Used to get the correct data from data manager)
    public int CurrentGunNum { get; private set; }

    GameManager gMan;
    ObjectPooler objPool;

   [Space]
    public float damageLogMultilpier;
    public float rangeLogMultilpier;
    public float fireRateLogMultilpier;

    // Change this if the gun equation changes
    public float MinDamage { get; private set; }
    public float MaxDamage { get; private set; }

    public float MinRange { get; private set; }
    public float MaxRange { get; private set; }

    public float MinFireRate { get; private set; }
    public float MaxFireRate { get; private set; }

    private void Start()
    {
        gMan = GameManager.instance;
        objPool = ObjectPooler.instance;

        recoilLerpMultiplier = (fireRate + 0.316f) / 0.149333f;

        slideOrigPos = slide.localPosition;

        // Set the weapons correct pos and rot.
        targetPos = transform.localPosition;
        slideTargetPos = slideOrigPos;

        CalculateGunBaseParameters();
    }

    private void FixedUpdate()
    {
        slideTargetPos = 
            Vector3.Lerp(slideTargetPos, slideOrigPos, Time.deltaTime * recoilLerpMultiplier);
        slide.localPosition = 
            Vector3.Lerp(slide.localPosition, slideTargetPos, Time.deltaTime * recoilLerpMultiplier);

        CheckIfCanShoot();
    }

    Vector3 targetRot;
    Vector3 targetPos;
    Vector3 slideTargetPos;
    public void Shoot( Vector3 projectileForward)
    {
        // Spawn Muzzle
        objPool.SpawnFromPool(
            "Simple_Muzzle",
            muzzle.transform.position,
            Quaternion.Euler(muzzle.forward));

        // Spawn Bullet and add projectileForward. 
        // ProjectileForward being calculated in 'PlayerController'.
        objPool.SpawnFromPool(
            "Simple_Bullet",
            muzzle.transform.position,
            Quaternion.Euler(muzzle.forward))
            .GetComponent<Rigidbody>().AddForce(
            projectileForward * 60, ForceMode.VelocityChange);
        // BE CAREFUL AND DONT CHANGE FORWARD MULTIPLIER!
        // If you change this one - you will need to chage the one in 
        //'PController.projectileForward' formula!

        PlayerCon().WeaponCon.ShotRecoil(recoilForce);

        // Set slide pos recoil.
        slideTargetPos -=
            Vector3.forward * recoilForce / 10;

        canShoot = false;
    }

    [HideInInspector]
    public bool canShoot;
    float slidePosDiffrence;
    void CheckIfCanShoot()
    {
        slidePosDiffrence = (slideOrigPos.z - slide.transform.localPosition.z) * 10;
        // Check if the transform rotation is back to normal - so can shoot again.
        if (!canShoot && slidePosDiffrence < .002f)
        {
            canShoot = true;
        }
    }

    // Gun parameters methods
    void CalculateGunBaseParameters()
    {
        MinDamage = damage;
        MinRange = range;
        MinFireRate = fireRate;

        MaxDamage = Mathf.Log(15, damageLogMultilpier) + MinDamage;
        MaxRange = Mathf.Log(15, rangeLogMultilpier) + MinRange;
        MaxFireRate = Mathf.Log(15, fireRateLogMultilpier) + MinFireRate;

        // Check which child num is this weapon in the weapons parent
        // Used to match the one in the json file!!

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
                CurrentGunNum = i;
        }

        UpdateGunParameters();
    }

    public void UpdateGunParameters()
    {
        damageUpgradeCount = gMan.DataManager.storeData.weaponsDamageUpgradesCount[CurrentGunNum];
        rangeUpgradeCount = gMan.DataManager.storeData.weaponsRangeUpgradesCount[CurrentGunNum];
        fireRateUpgradeCount = gMan.DataManager.storeData.weaponsFireRateUpgradesCount[CurrentGunNum];

        damage = Mathf.Log(damageUpgradeCount, damageLogMultilpier) + MinDamage;
        range = Mathf.Log(rangeUpgradeCount, rangeLogMultilpier) + MinRange;
        fireRate = Mathf.Log(fireRateUpgradeCount, fireRateLogMultilpier) + MinFireRate;
    }

    // IK objects used for this gun
    [Header("IK objects for this gun")]
    public Transform shoulderIK;
    public Transform leftHandIdleIK;

    public override void Attack(Vector3 projectileForward)
    {
        //base.Attack(projectileForward);
        Shoot(projectileForward);
    }

    public override bool CanAttack => canShoot;

    public override float WeaponRange => range;
}
