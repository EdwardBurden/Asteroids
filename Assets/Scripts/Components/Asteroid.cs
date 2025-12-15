using System;
using System.Collections;
using UnityEngine;

public class Asteroid : PooledGameObject
{
    [SerializeField] private Rotator m_rotator;
    [SerializeField] private DamageComponent m_damageComponent;
    [SerializeField] private HealthComponent m_healthComponent;

    public Vector3 direction;

    private AsteroidData m_data;

    public void Setup(AsteroidData data)
    {
        m_data = data;
        direction = new Vector3(UnityEngine.Random.Range(1f, -1f), UnityEngine.Random.Range(1f, -1f), 0);
        if (m_damageComponent != null)
        {
            m_damageComponent.Setup(data.Damage);
        }
        if (m_healthComponent != null) 
        {
            m_healthComponent.Setup(data.Health , data.InvunerableTimeMS);
            m_healthComponent.HealthDepleted += OnHealthDepleted;
        }
    }

    private void Update()
    {
        this.transform.position += direction * m_data.MaxSpeed *Time.deltaTime;
    }

    private void OnDisable()
    {
        if (m_healthComponent != null)
        {
            m_healthComponent.HealthDepleted -= OnHealthDepleted;
        }
    }

    private void OnHealthDepleted() 
    {
        Game.Instance.AsteroidManager.SpawnChildren(this, m_data);
        Game.Instance.AsteroidManager.SpawnCollisonFX(this.transform.position);
        Destroy();
    }
}
