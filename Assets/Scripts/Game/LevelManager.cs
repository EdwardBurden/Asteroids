using UnityEngine;

public class LevelManager : MonoBehaviour, IGameSystem
{
    [SerializeField] private SpriteRenderer m_background;
    [SerializeField] private Vector2 m_boundMargin = new Vector2(2, 2);

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
        Debug.Log($"{nameof(LevelManager)} Startup");
        var worldBottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        var worldTopRight = Camera.main.ViewportToWorldPoint(Vector3.one);
        var size = worldTopRight - worldBottomLeft;
        m_worldCenter = worldBottomLeft + (size / 2);
        m_worldSize = size;
    }

    public void ShutDown()
    {
        Debug.Log($"{nameof(LevelManager)} ShutDown");
    }

    public Vector2 GetRandomPointInBounds(Vector3 avoidPosition, float area, int maxAttempts = 3)
    {
        var attempt = 0;
        var success = false;
        var point = Vector3.zero;
        while (attempt < maxAttempts || !success)
        {
            var halfSize = (m_worldSize - m_boundMargin) / 2;
            var x = m_worldCenter.x + UnityEngine.Random.Range(-halfSize.x, halfSize.x);
            var y = m_worldCenter.y + UnityEngine.Random.Range(-halfSize.y, halfSize.y);
            point = new Vector2(x, y);
            if (Vector3.Distance(avoidPosition, point) > area)
            {
                success = true;
            }
            attempt++;

        }
        return point;
    }

    public Vector2 GetRandomPointInBounds()
    {
        var point = Vector3.zero;
        var halfSize = (m_worldSize - m_boundMargin) / 2;
        var x = m_worldCenter.x + UnityEngine.Random.Range(-halfSize.x, halfSize.x);
        var y = m_worldCenter.y + UnityEngine.Random.Range(-halfSize.y, halfSize.y);
        point = new Vector2(x, y);
        return point;
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

public bool IsGameObjectInBounds(GameObject levelBoundedObject)
{
    var flatPos = new Vector2(levelBoundedObject.transform.position.x, levelBoundedObject.transform.position.y);
    var halfSize = m_worldSize / 2;
    var isInBoundary = flatPos.x < (m_worldCenter.x + halfSize.x);
    isInBoundary &= flatPos.y < (m_worldCenter.y + halfSize.y);
    isInBoundary &= flatPos.x > (m_worldCenter.x - halfSize.x);
    isInBoundary &= flatPos.y > (m_worldCenter.y - halfSize.y);
    return isInBoundary;
}

public Vector3 CalculateMirroredPosition(GameObject levelBoundedObject)
{
    //TODO find simpler calculation
    var flatPos = new Vector2(levelBoundedObject.transform.position.x, levelBoundedObject.transform.position.y);
    var mirroredPos = flatPos;
    var halfSize = m_worldSize / 2;
    var min = m_worldCenter - halfSize;
    var max = m_worldCenter + halfSize;

    for (var i = 0; i < 2; i++) // for x and y
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

public void SetBackground(Sprite sprite)
{
    m_background.sprite = sprite;
}
}
