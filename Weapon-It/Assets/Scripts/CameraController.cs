using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singelton
    public static CameraController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    // Player
    Transform target;

    public float temp;
    Vector3 positionOffset;

    public float cameraMovementSpeed; // Used to multiply the deltatime of the position lerp.

    public float rotXConstraints;

    InputHandler inputH;

    void Start()
    {
        target = PlayerController.instance.transform;

        inputH = InputHandler.instance;

        // Set the offset as seen in the scene.
        positionOffset = transform.position;

        targetRot.x = transform.rotation.eulerAngles.x;
    }

    void Update()
    {
        MoveCamera();

        RotateCamera();
    }

    Vector3 targetRot;
    private void RotateCamera()
    {
        targetRot.y = Mathf.Lerp(rotXConstraints, -rotXConstraints, inputH.inputPrecentage.x);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), Time.deltaTime * 3);
    }

    Vector3 targetPos;
    private void MoveCamera()
    {
        targetPos =                                               // Set target position with offsets and small divide.
                    new Vector3(target.position.x / temp,         // X
                    target.position.y / 3.2f + positionOffset.y,  // Y
                    target.position.z + positionOffset.z);        // Z

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraMovementSpeed);
    }

}
