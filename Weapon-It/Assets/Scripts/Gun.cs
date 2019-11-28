using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public float gunRange = 30;
    public float recoilRcoveryMultiplier = 5;
    public float recoilForce = 10;

    public Transform slide;
    Vector3 slideOrigPos;

    public Transform muzzle;
    public Transform shellExit;

    ObjectPooler objPool;

    private void Start()
    {
        objPool = ObjectPooler.instance;

        slideOrigPos = slide.localPosition;

        // Set the weapons correct pos and rot.
        targetPos = transform.localPosition;
        slideTargetPos = slideOrigPos;
    }

    private void FixedUpdate()
    {
        // Lerp thorugh local Rot\Pos and slide Pos towards zero (to the regualr pos)
        #region Lerping
        targetRot = 
            Vector3.Lerp(targetRot, Vector3.zero, Time.deltaTime * recoilRcoveryMultiplier);
        transform.localRotation =
            Quaternion.Lerp(transform.localRotation,
            Quaternion.Euler(targetRot), Time.deltaTime * recoilRcoveryMultiplier * 4);

        targetPos = 
            Vector3.Lerp(targetPos, Vector3.zero, Time.deltaTime * recoilRcoveryMultiplier);
        transform.localPosition = 
            Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * recoilRcoveryMultiplier);

        slideTargetPos = 
            Vector3.Lerp(slideTargetPos, slideOrigPos, Time.deltaTime * recoilRcoveryMultiplier * 3);
        slide.localPosition = 
            Vector3.Lerp(slide.localPosition, slideTargetPos, Time.deltaTime * recoilRcoveryMultiplier * 3);
        #endregion

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

        #region Recoil
        // Set X rot recoil.
        targetRot -= transform.right * Random.Range(recoilForce * .7f, recoilForce) * 2;

        // Set Y rot Recoil.
        targetRot -= transform.up * Random.Range(-recoilForce, recoilForce) / 7f;

        // Set Z rot Recoil.
        targetRot -= transform.forward * Random.Range(-recoilForce, recoilForce) / 5;


        //Set pos recoil.
        targetPos -= transform.parent.forward * Random.Range(recoilForce / 2 * .7f, recoilForce / 3) / 5f;

        // Set slide pos recoil.
        slideTargetPos -=
            Vector3.forward * (Random.Range(recoilForce * .85f, recoilForce) / (recoilForce * 2.5f));

        #endregion

        canShoot = false;
    }

    [HideInInspector]
    public bool canShoot;
    float slidePosDiffrence;
    void CheckIfCanShoot()
    {
        slidePosDiffrence = (slideOrigPos.z - slide.transform.localPosition.z) * 10;
        // Check if the transform rotation is back to normal - so can shoot again.
        if (!canShoot && slidePosDiffrence < .01f)
        {
            canShoot = true;
        }
    }


    // Detect Collision with target - used from here beacuse the collider is on this obj.
    // (Check 'PlayerController' for more information)
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<PlayerController>().CollisionDetected();
    }

    // Used to override the Weapon stuff :)
    public override void Attack(Vector3 projectileForward)
    {
        //base.Attack(projectileForward);
        Shoot(projectileForward);

    }
    public override bool canAttack()
    {
        return canShoot;
    }

    public override float weaponRange ()
    {
        return gunRange;
    }
}
