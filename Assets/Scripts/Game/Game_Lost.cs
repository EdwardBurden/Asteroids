using UnityEngine;
using static Game;

public sealed partial class Game : MonoBehaviour
{
    private void Lost_OnEnter(GameState previousState)
    {
        m_playerInput.PauseInput();
        m_hud.ShowLostScreen();
    }

    private void Lost_OnUpdate()
    {

    }

    private void Lost_OnExit(GameState nextState)
    {
        m_playerInput.ResumeInput();
        m_hud.HideLostScreen();
    }
}
