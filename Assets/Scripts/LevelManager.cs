using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour , IGameSystem
{
    [SerializeField] private ObstacleSpawner m_obstacleSpawner;
    [SerializeField] private SpriteRenderer m_background;

    public delegate void OnObstaclesCleared();
    public event OnObstaclesCleared OnLevelCleared;

    private readonly List<Obstacle> m_spawnedObstacles = new();

    public void StartLevel(int level)
    {
        var gameData = App.Instance.GameData;
        var levelData = gameData.GetLevelData(level);
        m_background.sprite = levelData.Background;

        m_spawnedObstacles.Clear();
        m_obstacleSpawner.SpawnWave(levelData.MinObstacles , levelData.MaxObstacles);

    }

    public void Startup()
    {
        m_obstacleSpawner.Setup();
    }

    public void ShutDown()
    {
        m_obstacleSpawner.FreeAll();
    }
}
