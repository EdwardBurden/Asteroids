using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "GameData/AsteroidData")]
public class AsteroidData : ScriptableObject
{
    public string Name;
    public Asteroid Prefab;
    public int MinSpeed;
    public int MaxSpeed;
    public bool ColliderWithAsteroids = true;
    public float AliveTimeMS;

    public AsteroidData[] m_childAsteroids;


    //etc
}
