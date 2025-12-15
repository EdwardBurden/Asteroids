using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "GameData/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string Name;
    public Player Prefab;
    public int Health;
    public int InvunerableTimeMS;
    public float BulletSpawnIntervalMS;
    public BulletData Bullets;
    public Sprite PlayerIconAlive;
    public Sprite PlayerIconDead;
    public float Speed;
}
