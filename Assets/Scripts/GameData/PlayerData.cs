using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "GameData/PlayerData")]
public class PlayerData : SpaceObjectData<Player>
{
    [Header("Player Data")]
    public float BulletSpawnIntervalMS;
    public BulletData Bullets;
    public Sprite PlayerIconAlive;
    public Sprite PlayerIconDead;
    public float Speed;
}
