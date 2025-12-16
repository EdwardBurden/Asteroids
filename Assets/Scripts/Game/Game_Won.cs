using UnityEngine;

public sealed partial class Game : MonoBehaviour
{
    private void Won_OnEnter(GameState previousState)
    {
        Time.timeScale = 0.2f;
        m_playerInput.PauseInput();
        m_hud.ShowWonScreen();
        m_levelManager.SetBackground(App.Instance.GameData.WonBackground);
    }

    private void Won_OnUpdate()
    {

    }

    private void Won_OnExit(GameState nextState)
    {
        Time.timeScale = 1f;
        m_playerInput.ResumeInput();
        m_hud.HideWonScreen();
    }
}
