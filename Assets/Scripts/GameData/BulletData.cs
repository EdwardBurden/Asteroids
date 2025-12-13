using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "GameData/BulletData")]
public class BulletData : ScriptableObject
{
    public Bullet Prefab;
    public int Damage;
    public float AliveTimeMS;
    public float Speed;


}
