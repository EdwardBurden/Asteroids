using System.Collections;
using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;

public class Game : MonoBehaviour, IGameSystem
{
    //[SerializeField] ObstacleSpawner m_obstacleSpawner;
    [SerializeField] private LevelManager m_levelManager;

    private GameState m_gameState;


    public void StartGame() //TESTING
    {
        m_gameState = new GameState();
        m_gameState.Level = 0;
        m_gameState.Seed = App.Instance.GameData.UseSeed ? App.Instance.GameData.Seed : (int)System.DateTime.Now.Ticks;
        UnityEngine.Random.InitState(m_gameState.Seed);


        IEnumerator loop()
        {
            while (true)
            {
                m_levelManager.StartLevel(m_gameState.Level);
                yield return new WaitForSeconds(10);
                m_gameState.Level++;
            }
        }
        StartCoroutine(loop());

    }

    //looop
    //start a level, levelmanager to create a level, 
    //spawn in th eplayer
    //spawn in obstacles
    // go
    //listen to eveyrthign else



    public void OnPlayerDies() { } // told my playermanager


    public void OnLevelCleared() { } //told by levelmanager

    private void Update()
    {

    }

    public void Startup()
    {
        m_levelManager.Startup();
    }

    public void ShutDown()
    {
        m_levelManager.ShutDown();
    }

    private struct GameState
    {
        public int Level;
        public int Seed;

    }
}
