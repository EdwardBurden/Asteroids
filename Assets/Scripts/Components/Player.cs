using System;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [SerializeField] private Transform m_bulletRoot;
    [SerializeField] private HealthComponent m_healthComponent;
    [SerializeField] private PlayerMovementComponent m_movementComponent;
    [SerializeField] private DamageComponent m_damageComponent;

    public Action<int> Damaged;
    public Action<int> Healed;
    public Vector3 BulletRoot => m_bulletRoot.position;

    public void Setup(PlayerData m_data)
    {
        if (m_healthComponent != null)
        {
            m_healthComponent.Setup(m_data.Health, m_data.InvunerableTimeSeconds, recoveryTime: m_data.HealthRecoveryTime);
            m_healthComponent.DamageTaken += DamageTaken;
            m_healthComponent.HealthRecovered += healthRecovered;
        }

        if (m_damageComponent != null)
        {
            m_damageComponent.Setup(m_data.Damage, m_data.IgnoreLayers);
        }

        m_movementComponent.Setup(Vector2.zero, new MovementParameters(m_data.MaxSpeed, m_data.MinSpeed, m_data.Damping, m_data.Acceleration));
    }

    private void OnDisable()
    {
        if (m_healthComponent != null)
        {
            m_healthComponent.DamageTaken -= DamageTaken;
            m_healthComponent.HealthRecovered -= healthRecovered;
        }
    }

    private void DamageTaken(int damage)
    {
        Damaged?.Invoke(damage);
    }

    private void healthRecovered(int amount)
    {
        Healed?.Invoke(amount);
    }

    public void Move(Vector2 movement)
    {
        m_movementComponent.SetMovementInput(movement);
    }
}
