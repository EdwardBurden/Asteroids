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

    public GameStateData GAmeStateData => m_gameStateData;

    private GameStateData m_gameStateData;
    SimpleStateMachine<GameState> m_gameStatemachine = new SimpleStateMachine<GameState>();
    [SerializeField] private GameSceneReference m_gameSceneReference;
    public void Awake()
    {
        ServiceLocator.RegisterService<LevelManager>(this, m_gameSceneReference);
        ServiceLocator.RegisterService<GameInputManager>(this, m_gameSceneReference);
        ServiceLocator.RegisterService<PlayerController>(this, m_gameSceneReference);
        ServiceLocator.RegisterService<AsteroidManager>(this, m_gameSceneReference);
    }

    void OnDestroy()
    {
        ServiceLocator.UnRegisterService<LevelManager>();
        ServiceLocator.UnRegisterService<GameInputManager>();
        ServiceLocator.UnRegisterService<PlayerController>();
        ServiceLocator.UnRegisterService<AsteroidManager>();
    }

    public void Startup()
    {
        ServiceLocator.GetService<LevelManager>().Startup();
        ServiceLocator.GetService<GameInputManager>().Startup();
        ServiceLocator.GetService<PlayerController>().Startup();
        ServiceLocator.GetService<AsteroidManager>().Startup();
        m_gameSceneReference.m_hud.Startup();
        m_gameStatemachine.RegisterState(GameState.Loading, onUpdate: () => { m_gameStatemachine.ChangeState(GameState.Playing); }); //todo
        m_gameStatemachine.RegisterState(GameState.Playing, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
        m_gameStatemachine.RegisterState(GameState.Lost, Lost_OnEnter, Lost_OnUpdate, Lost_OnExit);
        m_gameStatemachine.RegisterState(GameState.Won, Won_OnEnter, Won_OnUpdate, Won_OnExit);
        m_gameStatemachine.RegisterState(GameState.Replay, onEnter: Replay_OnEnter);
        m_gameStatemachine.Init(GameState.Loading);
        //m_gameStatemachine.RegisterState(GameState.Paused, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
    }

    public void ShutDown()
    {

        ServiceLocator.GetService<LevelManager>().ShutDown();
        ServiceLocator.GetService<GameInputManager>().ShutDown();
        ServiceLocator.GetService<PlayerController>().ShutDown();
        ServiceLocator.GetService<AsteroidManager>().ShutDown();
        m_gameSceneReference.m_hud.ShutDown();
        m_gameStatemachine.Shutdown();
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
