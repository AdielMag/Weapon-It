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

    ObjectPooler objPool;

    private void Start()
    {
        objPool = ObjectPooler.instance;

        recoilLerpMultiplier = (fireRate + 0.316f) / 0.149333f;

        slideOrigPos = slide.localPosition;

        // Set the weapons correct pos and rot.
        targetPos = transform.localPosition;
        slideTargetPos = slideOrigPos;
    }

    private void FixedUpdate()
    {
        slideTargetPos = 
            Vector3.Lerp(slideTargetPos, slideOrigPos, Time.deltaTime * recoilLerpMultiplier);
        slide.localPosition = 
            Vector3.Lerp(slide.localPosition, slideTargetPos, Time.deltaTime * recoilLerpMultiplier);

        CheckIfCanShoot();
        //CalculateFireRate();
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

        if(tempFireRate == 0)
            timeOfStartShoot = Time.time;

        tempFireRate++;
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

    // IK objects used for this gun
    [Header("IK objects for this gun")]
    public Transform shoulderIK;
    public Transform leftHandIdleIK;

    float timeOfStartShoot;
    float tempFireRate;
    public void CalculateFireRate()
    {
        if (timeOfStartShoot + 10 < Time.time)
        {
            Debug.Log(tempFireRate);

            tempFireRate = 0;
        }
    }

    public override void Attack(Vector3 projectileForward)
    {
        //base.Attack(projectileForward);
        Shoot(projectileForward);
    }

    public override bool CanAttack => canShoot;

    public override float WeaponRange => range;
}
