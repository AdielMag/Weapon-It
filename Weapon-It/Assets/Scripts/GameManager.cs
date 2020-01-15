using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singelton + set data manager.
    public static GameManager instance;   
    private void Awake()
    {
        if (instance && instance != this)
            Destroy(instance.gameObject);

        instance = this;

        DontDestroyOnLoad(this);

        dataManager = GetComponent<JsonDataManager>();
    }

    // Variables that being set by external scripts
    public StoreManager SMan { get; set; }
    public LevelController LCon { get; set; }

    // Variables that this script sets.
    ObjectPooler objPooler;
    [HideInInspector]
    public JsonDataManager dataManager;

    private void Start()
    {
        objPooler = ObjectPooler.instance;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // The level con gets it when the level starts do know which level to start.
    public int CurrentLevel { get; set; }
    public void LoadLevel(int levelNum)
    {
        CurrentLevel = levelNum;
        SceneManager.LoadScene("Game");
    }
}
