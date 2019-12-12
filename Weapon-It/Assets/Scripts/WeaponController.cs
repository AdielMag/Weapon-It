using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool TargetDetected { get; private set; }
    // IK aim offset (not 100% accurate - need to add some angle offset)
    public Vector2 aimAngleOffset;

    public float WeaponRange { get; private set; }
    public Weapon CurrentWeapon { get; set; }

    RaycastHit currentTarget;   // Current target to aim and shoot at.
    float gunRecoilRcoveryMultiplier;

    PlayerController pCon;
    Animator anim;

    private void Update()
    {
        HandleTargetDetection();

        targetPos =
            Vector3.Lerp(targetPos, gunOrigLocalPos, Time.deltaTime * gunRecoilRcoveryMultiplier);
        shoulder.localPosition =
            Vector3.Lerp(shoulder.localPosition, targetPos, Time.deltaTime * gunRecoilRcoveryMultiplier);

        // Inverse kinematic offset
        if (TargetDetected)
        {
            currentTargetPos = Vector3.Lerp(currentTargetPos, targetFuturePos(), Time.deltaTime * 9);
            shoulder.LookAt(currentTargetPos);
            shoulder.eulerAngles +=
                transform.right * aimAngleOffset.y +
                transform.up * aimAngleOffset.x; 
        }
    }

    // Called after the weapon was instantiated
    public void Init()
    {
        pCon = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();

        // Set player controler in the gun script
        CurrentWeapon.GetComponent<Gun>().pCon = pCon;
        // Get range
        WeaponRange = CurrentWeapon.WeaponRange;

        gunRecoilRcoveryMultiplier =
            CurrentWeapon.GetComponent<Gun>().recoilRcoveryMultiplier;

        // Instantiate IK parent (Used for looking at the target)
        GameObject ikParent = new GameObject("IK Parent");
        ikParent.transform.SetParent(transform);
        ikParent.transform.localPosition = Vector3.zero;
        ikParent.transform.localScale = Vector3.one;

        // Instantiate and set shoulder ik
        shoulder = Instantiate(CurrentWeapon.GetComponent<Gun>().shoulderIK, ikParent.transform);

        rightHandIK = shoulder.GetChild(0);
        leftHandIK = shoulder.GetChild(1);

        // Set leftHandIdleIK from currentWeapon
        leftHandIdleIK = CurrentWeapon.GetComponent<Gun>().leftHandIdleIK;

        // Set gun orig pos and rot
        gunOrigLocalPos = shoulder.localPosition;

        targetPos = gunOrigLocalPos;

        enabled = true;
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

            // Check if can attack (+ if IK is in position)
            if (CurrentWeapon.CanAttack && targetAimIK > .98f)
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

    Vector3 gunOrigLocalPos;
    Vector3 targetPos;
    public void ShotRecoil(float recoilForce)
    {
        // Set pos recoil.
         targetPos -= transform.forward * Random.Range(recoilForce / 2 * .7f, recoilForce / 3) /3f;  
    }


    // -------- IK --------
    Transform shoulder;
    Transform rightHandIK;
    Transform leftHandIK;
    Transform leftHandIdleIK;

    Vector3 leftHandCurrentPos;
    Quaternion leftHandCurrentRot;

    Vector3 currentTargetPos; // Used to lerp the shoulder look at in update.
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

        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        if (layerIndex == 1)
        {
            // Set the target transform for the left hand ik
            Transform targetLHIKTransform;
            if (TargetDetected)
            {
                targetLHIKTransform = leftHandIK;
            }
            else
            {
                targetLHIKTransform = leftHandIdleIK;
            }
            
            // Lerp thorugh pos and rot
            leftHandCurrentPos =
            Vector3.Lerp(leftHandCurrentPos, targetLHIKTransform.position, Time.deltaTime * 45);
            leftHandCurrentRot =
            Quaternion.Lerp(leftHandCurrentRot, targetLHIKTransform.rotation, Time.deltaTime * 45);

            // Set the pos and rot
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandCurrentPos);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandCurrentRot);
        }

        // LookAt target lerping
        anim.SetLookAtWeight(1);
        lookAtTarget = Vector3.Lerp(lookAtTarget, 
            TargetDetected ? 
            currentTarget.transform.position - Vector3.up * 4 :
            transform.position + transform.forward * 20,
            Time.deltaTime * 5);

        anim.SetLookAtPosition(lookAtTarget);
    }
}
