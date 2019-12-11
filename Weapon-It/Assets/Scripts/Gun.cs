using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public float gunRange = 30;
    public float recoilRcoveryMultiplier = 5;
    public float recoilForce = 10;

    Vector3 gunOrigRot;
    Vector3 gunOrigPos;

    public Transform slide;
    Vector3 slideOrigPos;

    public Transform muzzle;
    public Transform leftHandIdleIK;
    public Transform shellExit;

    ObjectPooler objPool;

    private void Start()
    {
        objPool = ObjectPooler.instance;

        gunOrigPos = transform.localPosition;
        gunOrigRot = transform.localEulerAngles;

        slideOrigPos = slide.localPosition;

        // Set the weapons correct pos and rot.
        targetPos = transform.localPosition;
        slideTargetPos = slideOrigPos;
    }

    private void FixedUpdate()
    {

        // Lerp thorugh local Rot\Pos and slide Pos towards zero (to the regualr pos)
        /*
        targetRot = 
            Vector3.Lerp(targetRot, gunOrigRot, Time.deltaTime * recoilRcoveryMultiplier);
        transform.localRotation =
            Quaternion.Lerp(transform.localRotation,
            Quaternion.Euler(targetRot), Time.deltaTime * recoilRcoveryMultiplier * 4);

        targetPos = 
            Vector3.Lerp(targetPos, gunOrigPos, Time.deltaTime * recoilRcoveryMultiplier);
        transform.localPosition = 
            Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * recoilRcoveryMultiplier);
            */
        slideTargetPos = 
            Vector3.Lerp(slideTargetPos, slideOrigPos, Time.deltaTime * recoilRcoveryMultiplier * 3);
        slide.localPosition = 
            Vector3.Lerp(slide.localPosition, slideTargetPos, Time.deltaTime * recoilRcoveryMultiplier * 3);

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
            Vector3.forward * (Random.Range(recoilForce * .85f, recoilForce) / (recoilForce * 2.5f));

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
    
    // Used to override the Weapon stuff :)
    public override void Attack(Vector3 projectileForward)
    {
        //base.Attack(projectileForward);
        Shoot(projectileForward);
    }

    public override bool CanAttack => canShoot;

    public override float WeaponRange => gunRange;
}
