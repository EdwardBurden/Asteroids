using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputManager : MonoBehaviour, IGameSystem
{
    public enum SupportedInputDevices
    {
        GamePad,
        MouseAndKeyboard
    }

    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private float m_deadZoneAmount = 0.05f;
    private const string MoveActionName = "Move";
    private const string LookActionName = "Look";
    private const string ShootActionName = "Attack";

    public SupportedInputDevices CurrentInput { get; private set; }

    private InputAction m_move;
    private InputAction m_look;
    private InputAction m_shoot;
    private InputAction m_pause;
    private InputAction m_exit;

    public delegate void MouseDelegate(Vector2 movement, Vector3 mousePosition);
    public delegate void GamePadDelegate(Vector2 movement);

    public event GamePadDelegate OnMove;
    public event MouseDelegate OnLook_Mouse;
    public event GamePadDelegate OnLook_GamePad;
    public Action OnShootPressed;
    private bool m_initialised;
    private bool m_inputPaused;

    public void Startup()
    {
        m_move = InputSystem.actions.FindAction(MoveActionName);
        m_look = InputSystem.actions.FindAction(LookActionName);
        m_shoot = InputSystem.actions.FindAction(ShootActionName);
        m_shoot.performed += M_shoot_performed;
        m_initialised = true;
        m_inputPaused = false;
    }

    private void M_shoot_performed(InputAction.CallbackContext obj)
    {
        if (m_inputPaused)
            return;
        OnShootPressed?.Invoke();
    }

    public void ShutDown()
    {
        // throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (!m_initialised || m_inputPaused)
            return;

        M_playerInput_onControlsChanged();
        OnMovePerformed();
        OnLookPerformed();
    }

    private void M_playerInput_onControlsChanged( )
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
            Assert.Fail($"Device: {m_playerInput.currentControlScheme} is not supported");
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
              var value =   Mouse.current.position.ReadValue();
                Debug.Log(value);
                var world = Camera.main.ScreenToWorldPoint( new Vector3( value.x , value.y , 0));
                world = new Vector3((float)world.x, (float)world.y, 0);
                OnLook_Mouse?.Invoke(lookFrameData, world);
                break;
            default:
                break;
        }
    }

    internal void PauseInput()
    {
        m_inputPaused = true;
    }

    public void ResumeInput() 
    {
        m_inputPaused = false;
    }
}
