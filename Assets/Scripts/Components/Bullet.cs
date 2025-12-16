using System;
using System.Collections;
using UnityEngine;

public sealed class Bullet : PooledGameObject
{
    [SerializeField] private DamageComponent m_damageComponent;
    [SerializeField] private HealthComponent m_healthComponent;
    [SerializeField] private ConstantMovementComponent m_constantMovementComponent;

    private BulletData m_data;

    public void Setup(Vector3 forward, BulletData data)
    {
        m_data = data;
        m_constantMovementComponent.Setup(new MovementParameters(forward), m_data.Speed);
        if (m_damageComponent != null)
        {
            m_damageComponent.Setup(data.Damage, m_data.IgnoreLayers);
        }

        if (m_healthComponent != null)
        {
            m_healthComponent.Setup(health: 1, invunerableTime: 0, lifeTime: data.LifeTimeSeconds);
            m_healthComponent.HealthDepleted += OnHealthDepleted;
        }
    }

    private void OnHealthDepleted()
    {
        Destroy();
    }
}
