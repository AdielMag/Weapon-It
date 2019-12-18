using UnityEngine;

public class UIMethods : MonoBehaviour
{
    public GameObject levelWonWindow, levelLostWindow;

    GameManager gMan;

    private void Start()
    {
        gMan = GameManager.instance;

        gMan.UIManager = this;

    }


}