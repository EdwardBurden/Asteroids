using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
    private bool m_initialised;

    private readonly Dictionary<T, StateCallbacks<T>> m_states = new Dictionary<T, StateCallbacks<T>>();

    public void Init(T startState)
    {
        m_currentState = startState;
        m_statePending = false;
        m_initialised = true;
    }

    public void Update()
    {
        if (!m_initialised)
            return;

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
        m_statePending = true;
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

    public void Shutdown()
    {
        m_states.Clear();
        m_initialised= false;
    }
}
