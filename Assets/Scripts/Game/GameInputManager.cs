using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public enum SupportedInputDevices
{
    GamePad,
    MouseAndKeyboard
}

public class GameInputManager : IService
{
    private const string MoveActionName = "Move";
    private const string LookActionName = "Look";
    private const string ShootActionName = "Attack";

    private PlayerInput m_playerInput;
    private float m_deadZoneAmount = 0.05f;

    private InputAction m_move;
    private InputAction m_look;
    private InputAction m_shoot;
    private InputAction m_pause; //todo
    private InputAction m_exit; //todo

    public delegate void MouseDelegate(Vector2 movement, Vector3 mousePosition);
    public delegate void GamePadDelegate(Vector2 movement);

    public event GamePadDelegate OnMove;
    public event MouseDelegate OnLook_Mouse;
    public event GamePadDelegate OnLook_GamePad;
    public event Action OnShootPressed;

    private bool m_initialised;
    private bool m_inputPaused;
    public SupportedInputDevices CurrentInput { get; private set; }

    public GameInputManager(Game game, GameSceneReference gameSceneReference) 
    {
        m_playerInput = gameSceneReference.PlayerInput;
        m_deadZoneAmount = gameSceneReference.DeadZoneAmount;
    }

    public void Startup()
    {
        Debug.Log($"{nameof(GameInputManager)} StartUp");
        m_move = InputSystem.actions.FindAction(MoveActionName);
        m_look = InputSystem.actions.FindAction(LookActionName);
        m_shoot = InputSystem.actions.FindAction(ShootActionName);
        m_initialised = true;
        m_inputPaused = false;
    }

    public void ShutDown()
    {
        Debug.Log($"{nameof(GameInputManager)} ShutDown");
        m_initialised = false;
        m_inputPaused = true;
    }

    private void CheckControlChanged()
    {
        if (!m_initialised)
            return;

        if (m_playerInput.currentControlScheme == "Gamepad")
        {
            CurrentInput = SupportedInputDevices.GamePad;
        }
        else if (m_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            CurrentInput = SupportedInputDevices.MouseAndKeyboard;
        }
        else
        {
            Debug.LogError($"Device: {m_playerInput.currentControlScheme} is not supported");
        }
    }

    private void OnMovePerformed()
    {
        Vector2 moveFrameData = m_move.ReadValue<Vector2>();
        OnMove.Invoke(moveFrameData);
    }

    private void OnLookPerformed()
    {
        Vector2 lookFrameData = m_look.ReadValue<Vector2>();
        switch (CurrentInput)
        {
            case SupportedInputDevices.GamePad:
                if (lookFrameData.sqrMagnitude > m_deadZoneAmount * m_deadZoneAmount)
                {
                    OnLook_GamePad.Invoke(lookFrameData);
                }
                break;
            case SupportedInputDevices.MouseAndKeyboard:
                var value = Mouse.current.position.ReadValue();
                var world = Camera.main.ScreenToWorldPoint(new Vector3(value.x, value.y, 0));
                world = new Vector3(world.x, world.y, 0);
                OnLook_Mouse?.Invoke(lookFrameData, world);
                break;
            default:
                break;
        }
    }

    public void PauseInput()
    {
        m_inputPaused = true;
    }

    public void ResumeInput()
    {
        m_inputPaused = false;
    }

    void IService.Update()
    {
        if (!m_initialised || m_inputPaused)
            return;

        CheckControlChanged();
        OnMovePerformed();
        OnLookPerformed();
        if (m_shoot.IsPressed())
        {
            OnShootPressed?.Invoke();
        }
    }
}
