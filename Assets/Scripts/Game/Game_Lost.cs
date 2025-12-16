using UnityEngine;
using static Game;

public sealed partial class Game : MonoBehaviour
{
    private void Lost_OnEnter(GameState previousState)
    {
        Time.timeScale = 0.2f;
        m_playerInput.PauseInput();
        m_hud.ShowLostScreen();
        m_levelManager.SetBackground(App.Instance.GameData.LostBackground);
    }

    private void Lost_OnUpdate()
    {

    }

    private void Lost_OnExit(GameState nextState)
    {
        Time.timeScale = 1.0f;
        m_playerInput.ResumeInput();
        m_hud.HideLostScreen();
    }
}
