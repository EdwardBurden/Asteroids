using UnityEngine;

public sealed partial class Game 
{
    private void Won_OnEnter(GameState previousState)
    {
        Time.timeScale = 0.2f;
        ServiceLocator.GetService<GameInputManager>().PauseInput();
        m_gameSceneReference.m_hud.ShowWonScreen();
        ServiceLocator.GetService<LevelManager>().SetBackground(ServiceLocator.GetService<GameDataManager>().GameData.WonBackground);
    }

    private void Won_OnUpdate()
    {

    }

    private void Won_OnExit(GameState nextState)
    {
        Time.timeScale = 1f;
        ServiceLocator.GetService<GameInputManager>().ResumeInput();
        m_gameSceneReference.m_hud.HideWonScreen();
    }
}
