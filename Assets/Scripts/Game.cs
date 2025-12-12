using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.Audio.GeneratorInstance;

public class Game : MonoBehaviour, IGameSystem
{
    //[SerializeField] ObstacleSpawner m_obstacleSpawner;
    [SerializeField] private LevelManager m_levelManager;
    [SerializeField] private GameInputManager m_playerInput;
    [SerializeField] private PlayerController m_playerController;

    //   private GameState m_gameState;
    private GameStateData m_gameStateData;

    SimpleStateMachine<GameState> m_gameStatemachine = new SimpleStateMachine<GameState>();

    public void StartGame() //TESTING
    {
        m_gameStatemachine.ChangeState(GameState.Playing);
        // m_gameState = GameState.Playing;
        m_gameStateData.Level = 0;
        m_gameStateData.Seed = App.Instance.GameData.UseSeed ? App.Instance.GameData.Seed : (int)System.DateTime.Now.Ticks;
        UnityEngine.Random.InitState(m_gameStateData.Seed);

        ////TODO ADD STATE MACHINE FOR GAME STATES, test leaving game scene and reloading works

        IEnumerator loop()
        {
            while (true)
            {
                m_levelManager.StartLevel(m_gameStateData.Level);
                yield return new WaitForSeconds(10);
                m_gameStateData.Level++;
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
        m_playerInput.Startup();
        m_playerController.Startup();
        m_playerInput.OnMove += m_playerController.OnMoveInput;
        m_playerInput.OnLook_GamePad += m_playerController.OnLookGamePadInput;
        m_playerInput.OnLook_Mouse += m_playerController.OnLookMouseInput;
        //  m_gameStatemachine.RegisterState(GameState.Playing, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
        // m_gameStatemachine.RegisterState(GameState.Paused, Playing_OnEnter, Playing_OnUpdate, Playing_OnExit);
    }

    private void Playing_OnEnter(GameState previousState) { }
    private void Playing_OnUpdate() { }
    private void Playing_OnExit(GameState nextState) { }

    public void ShutDown()
    {
        m_levelManager.ShutDown();
        m_playerInput.ShutDown();
    }

    public enum GameState
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

    public struct StateCallbacks<T>
    {
        public Action<T> OnStateEnter;
        public Action OnStateUpdate;
        public Action<T> OnStateExit;
    }

    public class SimpleStateMachine<T> where T : Enum
    {
        private T m_currentState;
        private T m_pendingState;
        private bool m_statePending;

        private readonly Dictionary<T, StateCallbacks<T>> m_states = new Dictionary<T, StateCallbacks<T>>();

        public void Init(T startState)
        {
            m_currentState = startState;
            m_statePending = false;
        }

        public void Update()
        {
            if (m_statePending)
            {
                if (m_states.TryGetValue(m_currentState, out var currentCallbacks))
                {
                    currentCallbacks.OnStateExit?.Invoke(m_pendingState);
                }

                m_currentState = m_pendingState;
                if (m_states.TryGetValue(m_pendingState, out var pendingCallbacks))
                {
                    pendingCallbacks.OnStateEnter?.Invoke(m_currentState);
                }
                m_currentState = m_pendingState;
                m_statePending = false;
            }
            m_states[m_currentState].OnStateUpdate?.Invoke();
        }

        public void ChangeState(T state)
        {
            if (m_statePending)
                return; //ignore if already set for now // TODO make proper state machien later

            m_pendingState = state;
        }

        public void RegisterState(T state, StateCallbacks<T> callbacks)
        {
            Assert.IsTrue(!m_states.ContainsKey(state), "You registered same state twice");

            m_states[state] = callbacks;
        }

        public void RegisterState(T state, Action<T> onEnter = null, Action onUpdate = null, Action<T> onExit = null)
        {
            Assert.IsTrue(!m_states.ContainsKey(state), "You registered same state twice");

            m_states[state] = new StateCallbacks<T>() { OnStateEnter = onEnter, OnStateUpdate = onUpdate, OnStateExit = onExit };
        }
    }
}
