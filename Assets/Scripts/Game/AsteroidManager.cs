using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour, IGameSystem
{
    private const int AsteroidPoolSize = 20;
    private const int FXPoolSize = 10;
    private const int PlayerDistance = 5;

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

    public void SpawnWave(int level, Vector3 player)
    {
        var gameData = App.Instance.GameData;
        var levelData = gameData.LevelData[level];
        var asteroidsLength = levelData.asteroidlevelDatas.Length;
        for (int i = 0; i < gameData.LevelData[level].asteroidlevelDatas.Length; i++)
        {
            var data = gameData.LevelData[level].asteroidlevelDatas[i];
            var amountToSpawn = UnityEngine.Random.Range(data.MinAsteroids, data.MaxAsteroids);
            for (int j = 0; j < amountToSpawn; j++)
            {
                var point = Game.Instance.LevelManager.GetRandomPointInBounds(player, PlayerDistance); //todo move to 
                StartCoroutine(SpawnAsteroid(data, data.Asteroid, point));
            }

        }
    }

    private IEnumerator SpawnAsteroid(AsteroidlevelData asteroidlevelData, AsteroidData asteroidData, Vector3 position)
    {
        yield return new WaitForSeconds( asteroidlevelData.DelayAmountSeconds);
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

    public int AsteroidsRemaining()
    {
        return m_managerAsteroids.Count;
    }

    public void SpawnChildren(Asteroid asteroid, AsteroidData data)
    {
        foreach (var child in data.m_childAsteroids)
        {
           // SpawnAsteroid(child, asteroid.transform.position);
        }
    }
}