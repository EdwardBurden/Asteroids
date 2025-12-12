using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;


public class AsteroidManager : MonoBehaviour
{
    [SerializeField] private Transform m_poolParent;

    private readonly Dictionary<AsteroidData, GameObjectPool<Asteroid>> m_asteroidPools = new();

    private readonly HashSet<Asteroid> m_managerAsteroids = new HashSet<Asteroid>();

    public void Setup(int poolSize = 12)  //only call once
    {
        var gameData = App.Instance.GameData;
        foreach (AsteroidData obstacleDefinition in gameData.AsteroidData)
        {
            var pool = new GameObjectPool<Asteroid>();
            pool.Warm(obstacleDefinition.Prefab, poolSize, m_poolParent, deSpawnCallback: OnAsteroidDespawned);
            m_asteroidPools.Add(obstacleDefinition, pool);
        }
    }

    public void SpawnWave(int level)
    {
        var gameData = App.Instance.GameData;
        var amountToSpawn = UnityEngine.Random.Range(gameData.LevelData[level].MinAsteroids, gameData.LevelData[level].MaxAsteroids);
        for (int i = 0; i < amountToSpawn; i++)
        {
            var dataIndex = UnityEngine.Random.Range(0, gameData.AsteroidData.Length);
            var data = gameData.AsteroidData[dataIndex];
            SpawnAsteroid(data);
        }
    }

    private void SpawnAsteroid(AsteroidData asteroidData, Vector3 enemy, Vector3 damaged)
    {
        var asteroid = m_asteroidPools[asteroidData].Allocate();
        asteroid.Setup(asteroidData);
        Vector3 oppositeToDamage = (damaged - enemy).normalized;
        //todo need to explod away from center and source of damage
        asteroid.transform.position = damaged;
        m_managerAsteroids.Add(asteroid);

        asteroid.SetRandomThing(oppositeToDamage);
        asteroid.gameObject.SetActive(true);
    }

    private void SpawnAsteroid(AsteroidData asteroidData)
    {
        var asteroid = m_asteroidPools[asteroidData].Allocate();
        asteroid.Setup(asteroidData);
        asteroid.transform.position = Game.Instance.LevelManager.GetRandomPointInBounds();
        m_managerAsteroids.Add(asteroid);
        asteroid.gameObject.SetActive(true);

    }

    private void OnAsteroidDespawned(Asteroid asteroid)
    {
        m_managerAsteroids.Remove(asteroid);
    }


    internal int AsteroidsRemaining()
    {
        return m_managerAsteroids.Count;
    }

    internal void OnAsteroidHit(Collider2D other, Asteroid asteroid, AsteroidData data)
    {
        foreach (var child in data.m_childAsteroids)
        {
            SpawnAsteroid(child, other.transform.position, asteroid.transform.position);
        }
    }
}