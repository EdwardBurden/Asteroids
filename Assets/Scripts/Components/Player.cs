using System;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform m_bulletRoot;
    [SerializeField] private HealthComponent m_healthComponent;

    public Action Damaged;
    public Vector3 BulletRoot => m_bulletRoot.position;

    public void Setup(PlayerData m_data) 
    {
        m_healthComponent.Setup(m_data.Health, m_data.InvunerableTimeMS);
        m_healthComponent.DamageTaken += DamageTaken;
    }

    private void OnDisable()
    {
        m_healthComponent.DamageTaken -= DamageTaken;
    }


    private void DamageTaken(int damage) 
    {
        Damaged?.Invoke();
    }
}
