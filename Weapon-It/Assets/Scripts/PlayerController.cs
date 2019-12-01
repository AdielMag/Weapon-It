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
    public int movementRotationForce = 15;
    public float yOffset; // Offset weapon Y location.
    public float gameWidth;
    public float targetIndicatorYMultiplier = 20; // Multiply input.y precentage.


    GameManager gMan;
    [HideInInspector]
    public WeaponController weaponCon;
    Animator anim;
    InputHandler inputH;

    void Start()
    {
        gMan = GameManager.instance;
        weaponCon = GetComponent<WeaponController>();
        anim = GetComponent<Animator>();
        inputH = InputHandler.instance;


        // Set the look from the start 
        // (if not will lerp from Vector3.zero and will look weird)
        targetLookAt = transform.position
                + Vector3.forward * 20
                + posDelta.y * Vector3.up * movementRotationForce
                + posDelta.x * Vector3.right * movementRotationForce;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();

        HandleAnimations();
    }

    void HandleAnimations()
    {
        anim.SetFloat("Horizontal", posDelta.x * 5);
        anim.SetBool("Aim",weaponCon.TargetDetected);
    }

    Vector2 targetPos;
    void MovePlayer()
    {
        // Use input precentage to set width location.
        targetPos.x = Mathf.Lerp(-gameWidth, gameWidth, inputH.inputPrecentage.x);

        // Multiply precentage by the multiplier and add offset.
        targetPos.y = yOffset;

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
        if (weaponCon.TargetDetected)
        {
            lerpTargetVar = weaponCon.targetFuturePos();
        }
        else
        {
            // Set target for lerp with fixed forward and wth posDelta variables
            lerpTargetVar = transform.position
                + Vector3.forward * 20
                + posDelta.y * Vector3.up * movementRotationForce
                + posDelta.x * Vector3.right * movementRotationForce;
        }

        targetLookAt = Vector3.Lerp(targetLookAt, lerpTargetVar, Time.deltaTime * 10);
        targetLookAt.y = 0;     // Dont want the player yo rotate in the Y axis
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
}
