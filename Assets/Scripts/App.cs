using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class App
{
    private Game m_game;
    private Entrypoint m_entry;

    public static App Instance { get; private set; }

    public App(Entrypoint entrypoint)
    {
        m_entry = entrypoint;
        ServiceLocator.RegisterService<GameDataManager>(entrypoint);
        Instance = this;
    }

    ~App()
    {
        ServiceLocator.UnRegisterService<GameDataManager>();

    }

    //[SerializeField] private int m_gameSceneIndex;
    // [SerializeField] private GameObject m_loadingSpinner;
    //GameData m_gameData;

    // public GameData GameData => m_gameData;

    private WaitForSeconds m_sleep = new WaitForSeconds(0.5f);


    public void StartUp()
    {
        StartGame();
    }

    public void ShutDown()
    {
        //TODO cleanup app specific things
        //StartCoroutine(CloseGameScene()); //maybe dont need to do this
    }

    public void StartGame()
    {
        m_entry.StartCoroutine(LoadGame());
    }

    private IEnumerator CloseGameScene()
    {
        if (m_game != null)
        {
            m_game.ShutDown();
            var op = SceneManager.UnloadSceneAsync(m_entry.m_gameSceneIndex);
            op.allowSceneActivation = false;
            while (!op.isDone)
            {
                yield return null;
            }
        }
    }

    public IEnumerator LoadGame()
    {
        m_entry.m_loadingSpinner.SetActive(true);
        yield return m_entry.StartCoroutine(CloseGameScene());
        yield return m_sleep;
        var asyncOp = SceneManager.LoadSceneAsync(m_entry.m_gameSceneIndex, LoadSceneMode.Additive);
        asyncOp.allowSceneActivation = true;
        while (!asyncOp.isDone)
        {
            yield return null;
        }
        m_entry.m_loadingSpinner.SetActive(false);
        yield return m_sleep;
        var gameSceneReferences = GameObject.FindObjectsByType<Game>(FindObjectsSortMode.None);
        Assert.IsTrue(gameSceneReferences.Length == 1, "Only One game should exist in the game scene");
        m_game = gameSceneReferences[0];
        m_game.Startup();
        m_game.StartGame();
    }
}
