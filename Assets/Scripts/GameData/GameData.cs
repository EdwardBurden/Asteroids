using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Definitions/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private int m_seed;
    [SerializeField] private ObstacleData[] m_obstacleData; //todo have these collected  without manyall pulling in
    [SerializeField] private LevelData[] m_levelData;

    public ObstacleData[] ObstacleData => m_obstacleData;
    public LevelData[] LevelData => m_levelData;
    public bool UseSeed => m_seed != 0;
    public int Seed => m_seed;

    private void OnValidate()
    {
        //make sure only one of this exists in the project
        //make sure no two assets have have the same name;
    }

    public LevelData GetLevelData(int level) 
    {
        if (level < 0 || level > m_levelData.Length)
            return null;
        return m_levelData[level];
    }
}