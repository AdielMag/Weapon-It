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

    Vector3 targetRotation = Vector3.zero;
    float angleDiffFromForward;
    void RotatePlayer()
    {
        CalculatePosDelta();

        // Check the angle of the forward to the current rot
        angleDiffFromForward = Vector3.Angle(transform.forward, Vector3.forward);

        // Check if there's a target on sight
        if (currentTarget.transform != null)
        {
            // Get the X and Y diffrence between player pos and target pos
            Vector3 angleDiffTowardTarget = new Vector3(transform.position.x - currentTarget.transform.position.x
                ,transform.position.y - currentTarget.transform.position.y) * 3;

            // Rotate towards target.
            targetRotation = Vector3.Lerp(targetRotation, -Vector3.up * angleDiffTowardTarget.x  
                + Vector3.right * angleDiffTowardTarget.y, Time.deltaTime * 15);
        }
        else
        {
            // Multiply the vector by the axis(once for X second for Y) and the delta (the diffrence from last pos)
            targetRotation += (Vector3.up * posDelta.x - Vector3.right * posDelta.y * 1.5f) * 16;

            // Lerp the location to zero - so it return to the regular 
            // rotation (which is the forward -not coded as forward, made in the scene- in this case).
            targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * 15);

        }

        transform.rotation = Quaternion.Euler(targetRotation);
    }

    Vector2 lastPos, currentPos,posDelta;
    void CalculatePosDelta()                // Calculates the diffrence from this pos to last frame pos.
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
                currentWeapon.Attack();

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
}
