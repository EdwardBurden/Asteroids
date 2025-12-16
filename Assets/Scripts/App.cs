using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    [SerializeField] private int m_gameSceneIndex;
    [SerializeField] private GameObject m_loadingSpinner;
    [SerializeField] GameData m_gameData;

    public static App Instance { get; private set; }
    public GameData GameData => m_gameData;

    private WaitForSeconds m_sleep = new WaitForSeconds(0.5f);

    private Game m_game;

    private void Awake()
    {
        Assert.IsNull(Instance, "App should only be Awoken once at entrypoint");
        Instance = this;
    }

    public void StartUp() //app startup 
    {
        Assert.IsNotNull(m_gameData, "Game Data is required");
        StartGame();
    }

    public void ShutDown() //app shutdown
    {


    }

    public void StartGame()
    {
        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame()
    {
        m_loadingSpinner.SetActive(true);
        if (m_game != null)
        {
            m_game.ShutDown();
           var op =  SceneManager.UnloadSceneAsync(m_gameSceneIndex);
            op.allowSceneActivation = false;
            while (!op.isDone)
            {
                yield return null;
            }
        }
        yield return m_sleep;
        var asyncOp = SceneManager.LoadSceneAsync(m_gameSceneIndex, LoadSceneMode.Additive);
        asyncOp.allowSceneActivation = true;
        while (!asyncOp.isDone)
        {
            yield return null;
        }
        m_loadingSpinner.SetActive(false);

        yield return m_sleep;
        var game = GameObject.FindObjectsByType<Game>(FindObjectsSortMode.None);
        Assert.IsTrue(game.Length == 1, "Only One game should exist in the game scene");
        m_game = game[0];
        m_game.Startup();
        m_game.StartGame();
    }

    public void LeaveGame()
    {
        //tell game to 
        // todo unload game scene

    }
}
