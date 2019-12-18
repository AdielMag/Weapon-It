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

        DataManager = GetComponent<JsonDataManager>();
    }

    ObjectPooler objPooler;

    public JsonDataManager DataManager { get; private set; }

    private void Start()
    {
        objPooler = ObjectPooler.instance;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
