using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Definitions/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private int m_seed;
    [SerializeField] private AsteroidData[] m_asteroidData; //todo have these collected  without manyall pulling in
    [SerializeField] private LevelData[] m_levelData;
    [SerializeField] private PlayerData[] m_playerData; //for futrue player picking 
  public Sprite LostBackground;
    public Sprite WonBackground;

    public AsteroidData[] AsteroidData => m_asteroidData;
    public LevelData[] LevelData => m_levelData;   
    public PlayerData[]  PlayerData => m_playerData;
    public bool UseSeed => m_seed != 0;
    public int Seed => m_seed;

    public int MaxLevel => m_levelData.Length -1;
    private void OnValidate()
    {
        //make sure only one of this exists in the project
        //make sure no two assets have have the same name;
    }

    public LevelData GetLevelData(int level)
    {
        if (level < 0 || level >= m_levelData.Length)
            return null;
        return m_levelData[level];
    }
}