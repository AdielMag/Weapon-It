using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    #region Singelton
    public static InputHandler instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion


    // Divide input by this to get location by the precentage.
    int screenWidth;
    int screenHeight;

    public Vector2 inputPrecentage;

    void Start()
    {
        // Get screen measures.
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        inputPrecentage = new Vector2(.5f, .5f);
    }

    private void Update()
    {
#if UNITY_EDITOR
        // Divide the position by the screen height to get precentage based location.

        if (Input.GetMouseButton(0))
        {
            if (!CanGetTouchInput())
                return;

            inputPrecentage =
                new Vector2(Input.mousePosition.x / screenWidth, Input.mousePosition.y / screenHeight);

            // TEMPORARY - Clamp the movement.y to prevent the weapon from getting too high.
            // Replace it later with a logarithmic function that moves the weapon less when getting too high

            inputPrecentage.y = Mathf.Clamp(inputPrecentage.y, .05f, .5f);
        }
        else
            startedOnUi = false;
#else
        if (Input.touchCount != 0)
        {
            if (!CanGetTouchInput())
                return;

            inputPrecentage = new Vector2(
              x: Input.GetTouch(0).position.x / screenWidth,
              y: Input.GetTouch(0).position.y / screenHeight);
        }
        else
            startedOnUi = false;
#endif
    }

    bool startedOnUi;
    bool CanGetTouchInput()
    {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject() || startedOnUi)
        {
            startedOnUi = true;
            Debug.Log("OnUI");
            return false;
        }

        return true;
#else
         if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || startedOnUi)
        {
            startedOnUi = true;
            Debug.Log("OnUI");
            return false;
        }

        return true;
#endif
    }
}
