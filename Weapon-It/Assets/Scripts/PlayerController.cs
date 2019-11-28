using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singelton
    static public PlayerController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public float movementSpeed = 3;
    public float yOffset; // Offset weapon Y location.
    public float gameWidth;
    public float heightMultiplier; // Multiply input.y precentage.

    float weaponRange;

    RaycastHit currentTarget;   // Current target to aim and shoot at.

    InputHandler inputH;
    Weapon currentWeapon;

    void Start()
    {
        inputH = InputHandler.instance;
        currentWeapon = transform.GetChild(0).GetComponent<Weapon>();

        weaponRange = currentWeapon.weaponRange();
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        HandleTargetDetection();
    }


    Vector2 targetPos;
    void MovePlayer()
    {
        // Use input precentage to set width location.
        targetPos.x = Mathf.Lerp(-gameWidth, gameWidth, inputH.inputPrecentage.x); 

        // Multiply precentage by the multiplier and add offset.
        targetPos.y = yOffset + inputH.inputPrecentage.y * heightMultiplier;

        // Lerp position
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * movementSpeed);
    }

    Vector3 lerpTargetVar, targetLookAt;
    float angleDiffFromForward;
    void RotatePlayer()
    {
        CalculatePosDelta();

        // Check the angle of the forward to the current rot
        angleDiffFromForward = Vector3.Angle(transform.forward, Vector3.forward);

        // Check if there's a target on sight
        if (currentTarget.transform != null)
        {
            lerpTargetVar = currentTarget.transform.position;
        }
        else
        {
            lerpTargetVar = transform.position + Vector3.forward * 20;
        }

        targetLookAt = Vector3.Lerp(targetLookAt, lerpTargetVar, Time.deltaTime * 10);
        transform.LookAt(targetLookAt);
    }

    Vector2 lastPos, currentPos,posDelta;
    // Calculates the diffrence from this pos to last frame pos.
    void CalculatePosDelta()                
    {
        currentPos = transform.position;

        posDelta = currentPos - lastPos;

        lastPos = currentPos;
    }

    Vector3 rayHalfExtents = new Vector3(1.6f, 1.6f, .2f);
    public LayerMask targetLayerMask;
    public TargetIndiactor tarIndiactor;
    void HandleTargetDetection()
    {
        Physics.BoxCast(transform.position, rayHalfExtents / 2, Vector3.forward,
            out currentTarget, Quaternion.identity, weaponRange, targetLayerMask);

        // Check if there isn't a target
        if (currentTarget.transform == null)
        {
            Physics.BoxCast(transform.position, rayHalfExtents, Vector3.forward,
                out currentTarget, Quaternion.identity, weaponRange, targetLayerMask);
        }
        
        // Check again after second boxcast
        if (currentTarget.transform != null)
        {
            // Check if can shoot
            if (currentWeapon.canAttack())
                currentWeapon.Attack(projectileForward());

            // Set target Indicator location.
            tarIndiactor.SetLocation(currentTarget.transform.position);
            tarIndiactor.onTarget = true;
        }
        else
        {
            // Set target Indicator location.
            tarIndiactor.SetLocation(transform.position + transform.forward * weaponRange);
            tarIndiactor.onTarget = false;
        }
    }

    // Calculate projectile forward
    Vector3 projectileForward()
    {
        return transform.forward;
    }

    // Called by the Gun script 
    // (Because the collider is on him - for diffrent type of colliders for diffrent weapons)
    public void CollisionDetected()
    {
        Debug.Log("Dead");
    }

}
