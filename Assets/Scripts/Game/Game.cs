using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;

public sealed partial class Game : MonoBehaviour
{
    private enum GameState
    {
        Loading,
        Playing,
        Paused,
        Dead,
        Won,
        Leaving
    }
    private struct GameStateData //todo
    {
        public int Level;
        public int Seed;

    }

    //[SerializeField] ObstacleSpawner m_obstacleSpawner;
    [SerializeField] private LevelManager m_levelManager;
    [SerializeField] private GameInputManager m_playerInput;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private AsteroidManager m_asteroidManager;

    public static Game Instance { get; private set; }
    public LevelManager LevelManager => m_levelManager;

    public AsteroidManager AsteroidManager => m_asteroidManager;
    public GameInputManager InputManager => m_playerInput;

    private void Awake()
    {
        Instance = this;
    }

    //   private GameState m_gameState;
    private GameStateData m_gameStateData;

    SimpleStateMachine<GameState> m_gameStatemachine = new SimpleStateMachine<GameState>();


    public void StartGame()
    {
        m_gameStatemachine.ChangeState(GameState.Playing);
    }

    public void OnPlayerDies() { } // told my playermanager


    public void OnLevelCleared() { } //told by levelmanager

    private void Update()
    {
        m_gameStatemachine.Update();
    }

    public void Startup()
    {
        m_levelManager.Startup();
        m_playerInput.Startup();
        m_playerController.Startup();
        m_asteroidManager.Setup();
        m_gameStatemachine.RegisterState(GameState.Playing, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
        m_gameStatemachine.RegisterState(GameState.Loading, onUpdate : ()=> { m_gameStatemachine.ChangeState(GameState.Playing); }); //todo add method with wait time maybe
        m_gameStatemachine.Init(GameState.Loading);
        //m_gameStatemachine.RegisterState(GameState.Paused, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
    }

    public void ShutDown()
    {
        m_levelManager.ShutDown();
        m_playerInput.ShutDown();
        m_gameStatemachine.Shutdown();
    }
}
