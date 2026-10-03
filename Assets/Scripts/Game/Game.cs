using UnityEngine;
public struct GameStateData
{
    public int Level;
    public int Seed;
    public int Score;
    public int PlayerHealthRemaining;
}

public sealed partial class Game : MonoBehaviour
{
    private enum GameState
    {
        Loading,
        Playing,
        Paused, //todo
        Lost,
        Won,
        Replay
    }

    [SerializeField] private LevelManager m_levelManager;
    [SerializeField] private GameInputManager m_playerInput;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private AsteroidManager m_asteroidManager;
    [SerializeField] private HUD m_hud;

    public static Game Instance { get; private set; } //todo remove and replace with IGameSystem ServiceLocator
    public LevelManager LevelManager => m_levelManager;

    public AsteroidManager AsteroidManager => m_asteroidManager;
    public GameInputManager InputManager => m_playerInput;
    public PlayerController PlayerController => m_playerController;
    public GameStateData GAmeStateData => m_gameStateData;

    private GameStateData m_gameStateData;
    SimpleStateMachine<GameState> m_gameStatemachine = new SimpleStateMachine<GameState>();


    private void Awake()
    {
        Instance = this;
    }

    public void Startup()
    {
        m_levelManager.Startup();
        m_playerInput.Startup();
        m_playerController.Startup();
        m_asteroidManager.Startup();
        m_hud.Startup();
        m_gameStatemachine.RegisterState(GameState.Loading, onUpdate: () => { m_gameStatemachine.ChangeState(GameState.Playing); }); //todo
        m_gameStatemachine.RegisterState(GameState.Playing, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
        m_gameStatemachine.RegisterState(GameState.Lost, onEnter:Lost_OnEnter, onExit:Lost_OnExit);
        m_gameStatemachine.RegisterState(GameState.Won, Won_OnEnter, Won_OnUpdate, Won_OnExit);
        m_gameStatemachine.RegisterState(GameState.Replay, onEnter: Replay_OnEnter);
        m_gameStatemachine.Init(GameState.Loading);
        //m_gameStatemachine.RegisterState(GameState.Paused, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
    }

    public void ShutDown()
    {
        m_levelManager.ShutDown();
        m_playerInput.ShutDown();
        m_playerController.ShutDown();
        m_asteroidManager.ShutDown();
        m_gameStatemachine.Shutdown();
        m_hud.ShutDown();
    }

    public void StartGame()
    {
        m_gameStatemachine.ChangeState(GameState.Playing);
    }

    private void Update()
    {
        m_gameStatemachine.Update();
    }

    public void PlayerTakenDamage(int damageTaken)
    {
        m_gameStateData.PlayerHealthRemaining -= damageTaken;
    }

    public void PlayerGainedHealth(int amount)
    {
        m_gameStateData.PlayerHealthRemaining += amount;
    }

    public void CreditPlayerScore(int score)
    {
        m_gameStateData.Score += score;
    }

    public void LevelComplete()
    {
        m_gameStateData.Level++;
    }

    public void Replay()
    {
        m_gameStatemachine.ChangeState(GameState.Replay);
    }
}
