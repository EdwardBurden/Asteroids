using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Entrypoint : MonoBehaviour
{
    [SerializeField] public int m_gameSceneIndex;
    [SerializeField] public GameObject m_loadingSpinner;
    [SerializeField] public GameData m_gameData;

    private App m_app;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        m_app = new App(this);
    }

    private void OnDestroy()
    {
        //TODO
    }

    private void OnApplicationPause(bool pause)
    {
        //TODO
    }

    private void Start()
    {
        m_app.StartUp(); 
    }

    private void OnApplicationQuit()
    {
        m_app.ShutDown();
    }

    private void Update()
    {
        ServiceLocator.Update();
    }
}
