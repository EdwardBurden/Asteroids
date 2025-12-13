using System;
using System.Collections;
using UnityEngine;


public class Asteroid : PooledGameObject
{
    [SerializeField] private Rotator m_rotator;

    private WaitForSeconds m_aliveTime;
    public Vector3 direction;

    private AsteroidData m_data;

    public float TimeAlive { get; private set; }
    public void Setup(AsteroidData data)
    {
        m_data = data;
        TimeAlive = 0;
        m_aliveTime = new WaitForSeconds(data.AliveTimeMS / 1000);
        direction = new Vector3(UnityEngine.Random.Range(1f, -1f), UnityEngine.Random.Range(1f, -1f), 0);
    }

    private void OnEnable()
    {
        StartCoroutine(CountDownLife());
    }

    private IEnumerator CountDownLife()
    {
        yield return m_aliveTime;
        if (this.gameObject != null || this.isActiveAndEnabled)
        {
            Destroy();
        }
    }

    private void Update()
    {
        this.transform.position += direction * Time.deltaTime;
        TimeAlive += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TimeAlive < 0.2f)
            return;

        if (other.GetComponent<Bullet>() != null)
        {

            Game.Instance.AsteroidManager.OnAsteroidHit(other, this, m_data);

            Destroy();
        }
    }

    public void SetRandomThing(Vector2 direction)
    {
        // Normalize to avoid speed changes
        direction = direction.normalized;

        // Random angle between -90° and +90°
        float angle = UnityEngine.Random.Range(-5f, 5f);

        // Rotate and return
        direction = Rotate(direction, angle);
    }

    private static Vector2 Rotate(Vector2 v, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }


}
