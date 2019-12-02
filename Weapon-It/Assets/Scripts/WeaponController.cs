using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponSlot;
    public bool TargetDetected { get; private set; }
    // IK aim offset (not 100% accurate - need to add some angle offset)
    public Vector2 aimAngleOffset;

    public float WeaponRange { get; private set; }
    public Weapon CurrentWeapon { get; private set; }

    RaycastHit currentTarget;   // Current target to aim and shoot at.
    float gunRecoilRcoveryMultiplier;

    PlayerController pCon;
    Animator anim;

    private void Start()
    {
        pCon = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();

        // Get currentWeapon
        CurrentWeapon = weaponSlot.GetChild(0).GetComponent<Weapon>();
        // Set player controler in the gun script
        CurrentWeapon.GetComponent<Gun>().pCon = pCon;
        // Get range
        WeaponRange = CurrentWeapon.WeaponRange;

        gunRecoilRcoveryMultiplier = 
            CurrentWeapon.GetComponent<Gun>().recoilRcoveryMultiplier;

        rightHandIK = shoulder.GetChild(0);
        leftHandIK = shoulder.GetChild(1);

        // Set gun orig pos and rot
        gunOrigLocalRot = shoulder.localRotation.eulerAngles;
        gunOrigLocalPos = shoulder.localPosition;

        targetPos = gunOrigLocalPos;
        targetRot = gunOrigLocalRot;
    }

    private void Update()
    {
        HandleTargetDetection();

        // Lerp thorugh local Rot\Pos and slide Pos towards zero (to the regualr pos)
        targetRot =
            Vector3.Lerp(targetRot, gunOrigLocalRot, Time.deltaTime * gunRecoilRcoveryMultiplier);
        shoulder.localRotation =
            Quaternion.Lerp(shoulder.localRotation,
            Quaternion.Euler(targetRot), Time.deltaTime * gunRecoilRcoveryMultiplier * 4);

        targetPos =
            Vector3.Lerp(targetPos, gunOrigLocalPos, Time.deltaTime * gunRecoilRcoveryMultiplier);
        shoulder.localPosition =
            Vector3.Lerp(shoulder.localPosition, targetPos, Time.deltaTime * gunRecoilRcoveryMultiplier);

        // Inverse kinamtics works weird
        if (TargetDetected)
        {
            shoulder.LookAt(targetFuturePos());
            shoulder.eulerAngles +=
                transform.right * aimAngleOffset.y +
                transform.up * aimAngleOffset.x; 
        }
    }

    Vector3 rayHalfExtents = new Vector3(4f, 4f, .2f);
    public LayerMask targetLayerMask;
    public TargetIndiactor tarIndiactor;
    void HandleTargetDetection()
    {
        Vector3 originPoint = transform.position + Vector3.up * 6;
        Physics.BoxCast(originPoint, rayHalfExtents / 2, Vector3.forward,
            out currentTarget, Quaternion.identity, WeaponRange, targetLayerMask);

        // Check if there isn't a target
        if (currentTarget.transform == null)
        {
            // If there isn't -  try and box cast with larger 
            // (check for target that arent straight forward)
            Physics.BoxCast(originPoint, rayHalfExtents, Vector3.forward,
                out currentTarget, Quaternion.identity, WeaponRange, targetLayerMask);
        }

        // Check again after second boxcast
        if (currentTarget.transform != null)
        {
            TargetDetected = true;

            // Check if can shoot
            if (CurrentWeapon.CanAttack)
                CurrentWeapon.Attack(CurrentWeapon.transform.forward);

            // Set target Indicator location.
            tarIndiactor.SetLocation(currentTarget.transform.position);
            tarIndiactor.onTarget = true;
        }
        else
        {
            TargetDetected = false;

            // Set target Indicator location.
            Vector3 targetIndiactorPos =
                originPoint + transform.forward * WeaponRange;
            tarIndiactor.SetLocation(targetIndiactorPos);
            tarIndiactor.onTarget = false;
        }
    }

    // Calculate target future position
    float distanceFromTarget, travelTime;
    public Vector3 targetFuturePos()
    {
        // Get Distance
        distanceFromTarget = Vector3.Distance(
            transform.position,
            currentTarget.transform.position);

        // If the target is close - set the bullet speed super fast (to get current pos)
        if (distanceFromTarget < 40)

            travelTime = distanceFromTarget / 600;
        else
            // Calculae travel time
            travelTime = distanceFromTarget / 60;

        /* 
         * FuturePosition = 
         * Y offset
         * currentTargetPosition +
         * targetVelocity(movmentDirection and force) *
         * travelTime
        */

        Vector3 targetPos =
            currentTarget.transform.position +
            currentTarget.transform.GetComponent<Rigidbody>().velocity *
            travelTime;

        return targetPos;
    }

    Vector3 gunOrigLocalRot, gunOrigLocalPos;
    Vector3 targetRot, targetPos;
    public void ShotRecoil(float recoilForce)
    {
        // Set X rot recoil.
        targetRot -= shoulder.right * Random.Range(recoilForce * .7f, recoilForce) * 2;
        // Set Y rot Recoil.
        targetRot -= shoulder.up * Random.Range(-recoilForce, recoilForce) / 3f;
        // Set Z rot Recoil.
        targetRot -= shoulder.forward * Random.Range(-recoilForce, recoilForce) / 3;

        // Set pos recoil.
         targetPos -= transform.forward * Random.Range(recoilForce / 2 * .7f, recoilForce / 3) /3f;  
    }

    [Header("IK")]
    public Transform shoulder;
    Transform rightHandIK;
    Transform leftHandIK;

    Vector3 lookAtTarget;

    float targetAimIK = 1;
    private void OnAnimatorIK(int layerIndex)
    {
        targetAimIK = Mathf.Lerp
            (targetAimIK, TargetDetected ? 1 : 0, Time.deltaTime * 5);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, targetAimIK);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, targetAimIK);

        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandIK.position);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandIK.rotation);

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, targetAimIK);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, targetAimIK);

        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIK.position);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIK.rotation);

        anim.SetLookAtWeight(1);
        lookAtTarget = Vector3.Lerp(lookAtTarget, 
            TargetDetected ? 
            currentTarget.transform.position - Vector3.up * 4 :
            transform.position + transform.forward * 20,
            Time.deltaTime * 5);

        anim.SetLookAtPosition(lookAtTarget);
    }
}
