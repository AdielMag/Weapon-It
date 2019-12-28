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

    public Vector2 rawTouchPosDelta;

    // Divide input by this to get location by the precentage.
    public int ScreenWidth { get; private set; }
    int screenHeight;

    void Start()
    {
        // Get screen measures.
        ScreenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void Update()
    {
        CalculateTouchDelta();
#if UNITY_EDITOR
        // Divide the position by the screen height to get precentage based location.

        if (Input.GetMouseButton(0))
        {
            if (!CanGetTouchInput())
                return;
        }
        else
        {
            startedOnUi = false;
            targetPosDelta = Vector2.zero;
        }
        rawTouchPosDelta = targetPosDelta;
#endif
    }

    Vector2 lastTouchPos, currentTouchPos, targetPosDelta;
    // Calculates the mouse and touch delta (Diff from last frame pos)
    void CalculateTouchDelta()
    {
#if UNITY_EDITOR
        currentTouchPos.x = Input.mousePosition.x / ScreenWidth;
        currentTouchPos.y = Input.mousePosition.y / ScreenWidth;

        targetPosDelta = (currentTouchPos - lastTouchPos) * 13;

        lastTouchPos = currentTouchPos;

        if (Input.GetMouseButton(0))
        {
            if (!CanGetTouchInput())
                targetPosDelta = Vector2.zero;
        }
        else
        {
            startedOnUi = false;
            targetPosDelta = Vector2.zero;
        }
#else
        if (Input.touchCount != 0)
        {
            if (CanGetTouchInput())
            {
                targetPosDelta = Input.GetTouch(0).deltaPosition / 55;
                targetPosDelta.y = 0;
            }
            else
                targetPosDelta = Vector2.zero;
        }
        else
        {
            startedOnUi = false;
            targetPosDelta = Vector2.zero;
        }
#endif
        rawTouchPosDelta = targetPosDelta;
    }

    public bool startedOnUi; // Used to check if the touch started on the ui
    bool CanGetTouchInput()
    {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject() || startedOnUi)
        {
            startedOnUi = true;
            return false;
        }

        return true;
#else
         if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || startedOnUi)
        {
            startedOnUi = true;
            return false;
        }

        return true;
#endif
    }
}
