using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle", menuName = "GameData/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    public string Name;
    public Obstacle Prefab;
    public int MinSpeed;
    public int MaxSpeed;
    public int DamageDealt;
    public float AliveTimeMS;
    //etc
}
