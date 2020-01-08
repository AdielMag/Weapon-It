using UnityEngine;
using UnityEngine.UI;

public class Fortress : MonoBehaviour
{
    public Slider indicator;

    public Transform baseParent;

    GameManager gMan;
    LevelController levelCon;

    private void Start()
    {
        gMan = GameManager.instance;
        levelCon = LevelController.instance;

        SpawnBase();
    }

    void SpawnBase()
    {
        GameObject currentBase = baseParent.GetChild(gMan.DataManager.storeData.EquippedBase).gameObject;

        Instantiate(currentBase, transform);
    }

    public void TakeDamage(int damage)
    {
    }

    void FortressDestroyed()
    {
        levelCon.LevelLost();
    }

}
