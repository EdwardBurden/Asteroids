using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "GameData/LevelData")]
public class LevelData : ScriptableObject
{
    public string Name;
    public Sprite Background;
    public float MaxTime;

    public AsteroidlevelData[] asteroidlevelDatas; //Placeholder need proper system for this later
}

[Serializable]
public class AsteroidlevelData
{
    public int MinAsteroids;
    public int MaxAsteroids;
    public float SpawnChance;
    public float DelayAmountSeconds;
    public AsteroidData Asteroid;
}