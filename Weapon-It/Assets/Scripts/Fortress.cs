using UnityEngine;
using UnityEngine.UI;

public class Fortress : MonoBehaviour
{
    public Slider indicator;

    public Transform baseParent;

    public Base currentBase;

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
        GameObject targetBase = baseParent.GetChild(gMan.dataManager.storeData.EquippedBase).gameObject;

        currentBase = Instantiate(targetBase, transform).GetComponent<Base>();
    }

    public void TakeDamage(int damage)
    {
        currentBase.health -= damage;

        if (currentBase.health <= 0)
            FortressDestroyed();
    }

    void FortressDestroyed()
    {
        levelCon.LevelLost();
    }

}
