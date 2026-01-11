using UnityEngine;

public sealed partial class Game 
{
    private void Playing_OnEnter(GameState previousState)
    {
        //todo check last state
        m_gameStateData.Level = 0;
        m_gameStateData.Seed = ServiceLocator.GetService<GameDataManager>().GameData.UseSeed ? ServiceLocator.GetService<GameDataManager>().GameData.Seed : (int)System.DateTime.Now.Ticks;
        UnityEngine.Random.InitState(m_gameStateData.Seed);
        m_gameStateData.PlayerHealthRemaining = ServiceLocator.GetService<PlayerController>().CurrentPlayerData.Health;
        ServiceLocator.GetService<PlayerController>().ReadyPlayer();
        StartNextLevel();
        
    }

    private void StartNextLevel()
    {
        var levelData = ServiceLocator.GetService<GameDataManager>().GameData.GetLevelData(m_gameStateData.Level);
        ServiceLocator.GetService<LevelManager>().StartLevel(m_gameStateData.Level);
        ServiceLocator.GetService<AsteroidManager>().SpawnWave(m_gameStateData.Level, ServiceLocator.GetService<PlayerController>().PlayerPosition);
    }

    private void Playing_OnUpdate()
    {
        m_gameSceneReference.m_hud.UpdateHUD(m_gameStateData);

        if (m_gameStateData.PlayerHealthRemaining <= 0) 
        {
            m_gameStatemachine.ChangeState(GameState.Lost);
            return;
        }

        if (ServiceLocator.GetService<AsteroidManager>().AsteroidsRemaining() > 0)
            return;

        if (m_gameStateData.Level >= ServiceLocator.GetService<GameDataManager>().GameData.MaxLevel) 
        {
            m_gameStatemachine.ChangeState(GameState.Won);
            return;
        }

        LevelComplete();
        StartNextLevel();
      
    }

    private void Playing_OnExit(GameState nextState)
    {
        //todo cleanup
    }
}
