using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "GameData/BulletData")]
public class BulletData : SpaceObjectData<Bullet>
{
    [Header("Bullet Data")]
    public float Speed;
    public float ReloadTime;
}
