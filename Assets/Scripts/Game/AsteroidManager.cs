using System;
using System.Collections.Generic;
using UnityEngine;


public class AsteroidManager : MonoBehaviour, IGameSystem
{
    private const int AsteroidPoolSize = 20;
    private const int FXPoolSize = 10;

    [SerializeField] private Transform m_poolParent;
    [SerializeField] private PooledParticle m_collsionPrefab;

    private readonly GameObjectPool<PooledParticle> m_collisonParticles = new();
    private readonly Dictionary<AsteroidData, GameObjectPool<Asteroid>> m_asteroidPools = new();

    private readonly HashSet<Asteroid> m_managerAsteroids = new HashSet<Asteroid>();

    public void Startup()
    {
        Debug.Log($"{nameof(AsteroidManager)} StartUp");
        var gameData = App.Instance.GameData;
        foreach (AsteroidData obstacleDefinition in gameData.AsteroidData)
        {
            var pool = new GameObjectPool<Asteroid>();
            pool.Warm(obstacleDefinition.Prefab, AsteroidPoolSize, m_poolParent, deSpawnCallback: OnAsteroidDespawned);
            m_asteroidPools.Add(obstacleDefinition, pool);
        }

        m_collisonParticles.Warm(m_collsionPrefab, FXPoolSize, m_poolParent);
    }

    public void ShutDown()
    {
        Debug.Log($"{nameof(AsteroidManager)} ShutDown");
        foreach (var (key, pool) in m_asteroidPools)
        {
            pool.Clear();
        }
        m_collisonParticles.Clear();
    }

    public void SpawnWave(int level)
    {
        var gameData = App.Instance.GameData;
        var amountToSpawn = UnityEngine.Random.Range(gameData.LevelData[level].MinAsteroids, gameData.LevelData[level].MaxAsteroids);
        for (int i = 0; i < amountToSpawn; i++)
        {
            var dataIndex = UnityEngine.Random.Range(0, gameData.AsteroidData.Length);
            var data = gameData.AsteroidData[dataIndex];
            var point = Game.Instance.LevelManager.GetRandomPointInBounds();
            SpawnAsteroid(data, point);
        }
    }

    private void SpawnAsteroid(AsteroidData asteroidData, Vector3 position)
    {
        var asteroid = m_asteroidPools[asteroidData].Allocate();
        asteroid.Setup(asteroidData);
        asteroid.transform.position = position;
        m_managerAsteroids.Add(asteroid);
        asteroid.gameObject.SetActive(true);
    }

    public void SpawnCollisonFX(Vector3 position)
    {
        var particle = m_collisonParticles.Allocate();
        particle.transform.position = position;
        particle.gameObject.SetActive(true);
    }

    private void OnAsteroidDespawned(Asteroid asteroid)
    {
        m_managerAsteroids.Remove(asteroid);
    }

    internal int AsteroidsRemaining()
    {
        return m_managerAsteroids.Count;
    }

    public void SpawnChildren(Asteroid asteroid, AsteroidData data)
    {
        foreach (var child in data.m_childAsteroids)
        {
            SpawnAsteroid(child, asteroid.transform.position);
        }
    }
}