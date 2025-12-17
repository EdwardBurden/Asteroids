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

    public AsteroidlevelData[] asteroidlevelDatas; //Placeholder need proper system for this later
}

[Serializable]
public class AsteroidlevelData
{
    public float SpawnChance;
    public AsteroidData Asteroid;
}