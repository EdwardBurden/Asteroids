using System;
using System.Collections;
using UnityEngine;

public class Bullet : PooledGameObject
{

    private WaitForSeconds m_aliveTime;
    private Vector2 direction;

    private BulletData m_data;
    public float TimeAlive { get; private set; }



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
        this.transform.position += new Vector3(direction.x , direction.y, 0) * m_data.Speed* Time.deltaTime;
        TimeAlive += Time.deltaTime;
    }

    internal void Move(Vector3 forward, BulletData data)
    {
        m_data = data;
        TimeAlive = 0;
        m_aliveTime = new WaitForSeconds(data.AliveTimeMS / 1000);
        direction = forward;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            return;
        }
        Destroy();
    }
}
