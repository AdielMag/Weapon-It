using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public GameObject levelWonWindow;
    public GameObject levelLostWindow;
    public GameObject pauseWindow;

    public GameObject pauseButton;

    public Image playerLogo;
    public CircleSlider timeLeft;
    public Slider fortressHealth;

    LevelController levelCon;

    private void Start()
    {
        levelCon = LevelController.instance;

        levelCon.uIManager = this;

        ResetUI();
    }

    public void ResetUI()
    {
        timeLeft.value = 1;
        fortressHealth.value = 1;
    }

    public void PauseLevel()
    {
        levelCon.currentlyPlaying = false;

        pauseButton.SetActive(false);
        pauseWindow.SetActive(true);
    }

    public void UnPauseLevel()
    {
        levelCon.currentlyPlaying = true;

        pauseButton.SetActive(true);
        pauseWindow.SetActive(false);
    }
}