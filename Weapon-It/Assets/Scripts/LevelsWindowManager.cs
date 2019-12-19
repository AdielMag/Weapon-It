using UnityEngine;
using UnityEngine.UI;

public class LevelsWindowManager : MonoBehaviour
{
    int maxLevel;
    GameObject levelPrefab;

    GameManager gMan;
    private void Start()
    {
        gMan = GameManager.instance;
        maxLevel = gMan.DataManager.gamePlayData.playerHighestLevel;

        levelPrefab = transform.GetChild(2).GetChild(0).gameObject;

        UpdateLevelsButtons();
    }

    Color targetColor;
    void UpdateLevelsButtons()
    {
        for (int i = 1; i <= 20; i++)
        {
            GameObject newLevelButton = Instantiate(levelPrefab, transform.GetChild(2));
            newLevelButton.transform.GetChild(1).GetComponent<Text>().text = i.ToString();

            if (i > maxLevel)
            {
                targetColor =
                     newLevelButton.transform.GetChild(0).GetComponent<Image>().color;
                targetColor.a *= .3f;

                newLevelButton.transform.GetChild(0).GetComponent<Image>().color = targetColor;
                newLevelButton.transform.GetComponent<Button>().interactable = false;
            }
            else
            {
                SetButtonAction(newLevelButton.transform.GetComponent<Button>(), i);
            }
        }

        Destroy(levelPrefab);
    }

    void SetButtonAction(Button button, int levelNum)
    {
        gMan.CurrentLevel = levelNum;

        button.onClick.AddListener(gMan.LoadLevel);

    }
}
