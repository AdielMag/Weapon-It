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
    public float heightMultiplier; // Multiply input.y precentage 

    InputHandler inputH;

    void Start()
    {
        inputH = InputHandler.instance;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
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
    void RotatePlayer()
    {
        CalculatePosDelta();

        // Multiply the vector by the axis(once for X second for Y) and the delta (the diffrence from last pos)
        targetRotation += (Vector3.up * posDelta.x - Vector3.right * posDelta.y * 2f) * 11;

        // Lerp the location to zero - so it return to the regular 
        // rotation (which is the forward -not coded as forward, made in the scene- in this case).
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * 15);

        transform.rotation = Quaternion.Euler(targetRotation);

    }

    Vector2 lastPos, currentPos,posDelta;
    void CalculatePosDelta()                // Calculates the diffrence from this pos to last frame pos.
    {
        currentPos = transform.position;

        posDelta = currentPos - lastPos;

        lastPos = currentPos;
    }
}
