using UnityEngine;

public sealed partial class Game : MonoBehaviour
{
    private void Won_OnEnter(GameState previousState)
    {
        m_playerInput.PauseInput();
        m_hud.ShowWonScreen();
    }

    private void Won_OnUpdate()
    {

    }

    private void Won_OnExit(GameState nextState)
    {
        m_playerInput.ResumeInput();
        m_hud.HideWonScreen();
    }
}
