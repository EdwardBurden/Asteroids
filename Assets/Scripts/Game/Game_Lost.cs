using UnityEngine;

public sealed partial class Game
{
    private void Lost_OnEnter(GameState previousState)
    {
        Time.timeScale = 0.2f;
        ServiceLocator.GetService<GameInputManager>().PauseInput();
        m_gameSceneReference.m_hud.ShowLostScreen();
        ServiceLocator.GetService<LevelManager>().SetBackground(ServiceLocator.GetService<GameDataManager>().GameData.LostBackground);
    }

    private void Lost_OnUpdate()
    {

    }

    private void Lost_OnExit(GameState nextState)
    {
        Time.timeScale = 1.0f;
        ServiceLocator.GetService<GameInputManager>().ResumeInput();
        m_gameSceneReference.m_hud.HideLostScreen();
    }
}
