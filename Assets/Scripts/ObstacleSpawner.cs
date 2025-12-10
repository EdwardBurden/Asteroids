using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform m_poolParent;

    private readonly Dictionary<ObstacleData, GameObjectPool<Obstacle>> m_obstaclePools = new();

    private readonly List<GameObject> m_obstacles = new List<GameObject>();

    public void Setup(int poolSize = 12)  //only call once
    {
        var gameData = App.Instance.GameData;
        foreach (ObstacleData obstacleDefinition in gameData.ObstacleData)
        {
            var pool = new GameObjectPool<Obstacle>();
            pool.Warm(obstacleDefinition.Obstacle, poolSize, m_poolParent);
            m_obstaclePools.Add(obstacleDefinition, pool);
        }
    }

    public void SpawnWave(int min, int max)
    {
        var gameData = App.Instance.GameData;
        var amountToSpawn = UnityEngine.Random.Range(min, max);
        for (int i = 0; i < amountToSpawn; i++)
        {
            var dataIndex = UnityEngine.Random.Range(0, gameData.ObstacleData.Length);
            var data = gameData.ObstacleData[dataIndex];
            var obstacle = m_obstaclePools[data].Allocate();
            obstacle.Setup(data);
            //toto do soemthing
        }
    }

    internal void FreeAll()
    {
        //todo destory all and clear pools
    }
}