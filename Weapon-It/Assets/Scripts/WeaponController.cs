using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponSlot;

    public bool TargetDetected { get; private set; }

    RaycastHit currentTarget;   // Current target to aim and shoot at.

    public float WeaponRange { get; private set; }
    public Weapon CurrentWeapon { get; private set; }

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
    }

    private void Update()
    {
        HandleTargetDetection();
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
                CurrentWeapon.Attack(transform.forward);

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
            Vector3.up * -2 +
            currentTarget.transform.position +
            currentTarget.transform.GetComponent<Rigidbody>().velocity *
            travelTime;

        return targetPos;
    }

    [Header("IK")]
    public Transform rightHandIK;
    public Transform leftHandIK;

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
    }
}
