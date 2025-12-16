using System;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public sealed class Player : MonoBehaviour
{
    [SerializeField] private Transform m_bulletRoot;
    [SerializeField] private HealthComponent m_healthComponent;
    [SerializeField] private PlayerMovementComponent m_movementComponent;

    public Action<int> Damaged;
    public Vector3 BulletRoot => m_bulletRoot.position;

    public void Setup(PlayerData m_data)
    {
        m_healthComponent.Setup(m_data.Health, m_data.InvunerableTimeSeconds);
        m_healthComponent.DamageTaken += DamageTaken;
        m_movementComponent.Setup(Vector2.zero, new MovementParameters(m_data.MaxSpeed, m_data.MinSpeed, m_data.Damping, m_data.Acceleration));
    }

    private void OnDisable()
    {
        m_healthComponent.DamageTaken -= DamageTaken;
    }


    private void DamageTaken(int damage)
    {
        Damaged?.Invoke(damage);
    }

    internal void Move(Vector2 movement)
    {
        m_movementComponent.SetMovementInput(movement);
    }
}
