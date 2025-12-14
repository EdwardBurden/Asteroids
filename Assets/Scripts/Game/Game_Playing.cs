using UnityEngine;

public sealed partial class Game : MonoBehaviour
{
    private void Playing_OnEnter(GameState previousState)
    {
        m_gameStateData.Level = 0;
        m_gameStateData.Seed = App.Instance.GameData.UseSeed ? App.Instance.GameData.Seed : (int)System.DateTime.Now.Ticks;
        UnityEngine.Random.InitState(m_gameStateData.Seed);
        m_gameStateData.PlayerHealthRemaining = m_playerController.CurrentPlayerData.Health;
        StartNextLevel();
    }

    private void StartNextLevel()
    {
        var levelData = App.Instance.GameData.GetLevelData(m_gameStateData.Level);
        m_levelManager.StartLevel(m_gameStateData.Level);
        m_asteroidManager.SpawnWave(m_gameStateData.Level);
    }


    private void Playing_OnUpdate()
    {
        m_hud.UpdateHUD(m_gameStateData);

        if (m_gameStateData.PlayerHealthRemaining <= 0) 
        {
            m_gameStatemachine.ChangeState(GameState.Lost);
            return;
        }

        if (m_asteroidManager.AsteroidsRemaining() > 0)
            return;

        if (m_gameStateData.Level >= App.Instance.GameData.MaxLevel) 
        {
            m_gameStatemachine.ChangeState(GameState.Won);
            return;
        }

        m_gameStateData.Level++;
        StartNextLevel();
    }
    private void Playing_OnExit(GameState nextState)
    {
        //todo cleanup
    }
}
