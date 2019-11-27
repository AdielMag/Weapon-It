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

    public Transform shellExit;

    private void Start()
    {
        slideOrigPos = slide.localPosition;

        // Set the weapons correct pos and rot.
        targetPos = transform.localPosition;
        slideTargetPos = slideOrigPos;

    }

    private void FixedUpdate()
    {
        targetRot = Vector3.Lerp(targetRot, Vector3.zero, Time.deltaTime * recoilRcoveryMultiplier);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * 10);

        targetPos = Vector3.Lerp(targetPos, Vector3.zero, Time.deltaTime * recoilRcoveryMultiplier);
        transform.localPosition = 
            Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * recoilRcoveryMultiplier);

        slideTargetPos = Vector3.Lerp(slideTargetPos, slideOrigPos, Time.deltaTime * recoilRcoveryMultiplier * 3);
        slide.localPosition = 
            Vector3.Lerp(slide.localPosition, slideTargetPos, Time.deltaTime * recoilRcoveryMultiplier * 3);

       // if (Input.GetMouseButtonDown(0))
       //     Shoot();

        CheckIfCanShoot();
    }

    Vector3 targetRot;
    Vector3 targetPos;
    Vector3 slideTargetPos;
    public void Shoot()
    {

        // Set X rot recoil.
        targetRot -= transform.right * Random.Range(recoilForce * .7f, recoilForce) * 2;

        // Set Y rot Recoil.
        targetRot -= transform.up * Random.Range(-recoilForce, recoilForce) / 3f;

        // Set Z rot Recoil.
        targetRot -= transform.forward * Random.Range(-recoilForce, recoilForce) / 2;


        //Set pos recoil.
        targetPos -= transform.parent.forward * Random.Range(recoilForce / 2 * .7f, recoilForce / 3) / 5f;

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
        if (!canShoot && slidePosDiffrence < .02f)
        {
            canShoot = true;
        }
    }

    // Used to ovverirde the Weapon stuff :)
    public override void Attack()
    {
        Shoot();
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
