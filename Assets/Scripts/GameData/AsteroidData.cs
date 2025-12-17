using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "GameData/AsteroidData")]    
public class AsteroidData : SpaceObjectData<Asteroid>
{
    [Header("Asteroid Data")]
    public float MaxSpeed;
    public float MinSpeed;
    public int Score;
    public AsteroidData[] m_childAsteroids;
}
