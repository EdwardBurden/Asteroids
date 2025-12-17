using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrypoint : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
        App.Instance.StartUp(); 
    }

    private void OnApplicationQuit()
    {
        App.Instance.ShutDown();
    }
}
