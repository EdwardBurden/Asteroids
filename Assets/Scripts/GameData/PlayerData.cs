using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "GameData/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string Name;
    public Player Prefab;
    public int Health;
    public float BulletSpawnIntervalMS;
    public BulletData Bullets;

}
