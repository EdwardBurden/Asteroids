using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "GameData/LevelData")]
public class LevelData : ScriptableObject
{
    public string Name;
    public Sprite Background;
    public int MinAsteroids;
    public int MaxAsteroids;
    public float MaxTime;

    public AsteroidlevelData[] asteroidlevelDatas;

    //tood have list of obstacles for this level
    // phaes? maybe use a curve to define amoutn of obstacles to spawn rate
}

[Serializable]
public class AsteroidlevelData
{
    public float SpawnChance;
    public AsteroidData Asteroid;
}