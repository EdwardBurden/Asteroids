using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class LevelManager : MonoBehaviour, IGameSystem
{
    [SerializeField] private SpriteRenderer m_background;
    [SerializeField] private Vector2 m_boundMargin = new Vector2(2, 2);

    public delegate void OnObstaclesCleared();
    public event OnObstaclesCleared OnLevelCleared;

    private Vector2 m_worldSize;
    private Vector2 m_worldCenter; //todo make vec

    public void StartLevel(int level)
    {
        var gameData = App.Instance.GameData;
        var levelData = gameData.GetLevelData(level);
        m_background.sprite = levelData.Background;
    }

    public void Startup()
    {
        var worldBottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        var worldTopRight = Camera.main.ViewportToWorldPoint(Vector3.one);
        var size = worldTopRight - worldBottomLeft;
        m_worldCenter = worldBottomLeft + (size / 2);
        m_worldSize = size;

    }

    public void ShutDown()
    {
        //m_obstacleSpawner.FreeAll();
    }

    public Vector2 GetRandomPointInBounds()
    {
        var halfSize = (m_worldSize - m_boundMargin) / 2;
        var x = m_worldCenter.x + UnityEngine.Random.Range(-halfSize.x, halfSize.x);
        var y = m_worldCenter.y + UnityEngine.Random.Range(-halfSize.y, halfSize.y);
        return new Vector2(x, y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(m_worldCenter, Vector3.one);
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                var target = m_worldCenter + new Vector2(i * m_worldSize.x / 2, j * m_worldSize.y / 2);
                Gizmos.DrawSphere(target, 1);
            }
        }
    }

    internal bool IsGameObjectInBounds(GameObject levelBoundedObject)
    {
        var flatPos = new Vector2(levelBoundedObject.transform.position.x, levelBoundedObject.transform.position.y);
        var halfSize = m_worldSize / 2;
        return (flatPos.x < m_worldCenter.x + halfSize.x && flatPos.y < m_worldCenter.y + halfSize.y && flatPos.x > m_worldCenter.x - halfSize.x && flatPos.x > m_worldCenter.y - halfSize.y);
    }

    internal Vector3 CalculateMirroredPosition(GameObject levelBoundedObject)
    {
        //TODO find simpler calculation
        var flatPos = new Vector2(levelBoundedObject.transform.position.x, levelBoundedObject.transform.position.y);
       var  mirroredPos = flatPos;
        var halfSize = m_worldSize / 2;
        var min = m_worldCenter - halfSize;
        var max = m_worldCenter + halfSize;

        for (var i = 0; i < 2; i++) //for 2, i hate writing basically the same code twice
        {
            if (flatPos[i] < min[i])
            {
                mirroredPos[i] = max[i];
            }
            else if (flatPos[i] > max[i])
            {
                mirroredPos[i] = min[i];
            }
        }
        return mirroredPos;
    }
}
