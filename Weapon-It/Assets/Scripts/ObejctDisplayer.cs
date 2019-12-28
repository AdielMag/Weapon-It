using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObejctDisplayer : MonoBehaviour
{
    public float rotationForce, rotationLerpSpeed;
    Vector3 targetRotation,currentRotation;

    InputHandler inputH;

    private void Start()
    {
        inputH = GetComponent<InputHandler>();
    }

    private void Update()
    {
        targetRotation += Vector3.up * -inputH.rawTouchPosDelta.x * rotationForce;

        currentRotation =
            Vector3.Lerp(currentRotation,
            targetRotation,
            Time.deltaTime * rotationLerpSpeed);

        transform.localEulerAngles = currentRotation;
    }
}
