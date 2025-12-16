using UnityEngine;

public sealed class Asteroid : PooledGameObject
{
    [SerializeField] private DamageComponent m_damageComponent;
    [SerializeField] private HealthComponent m_healthComponent;
    [SerializeField] private ConstantMovementComponent m_constantMovementComponent;

    private AsteroidData m_data;

    public void Setup(AsteroidData data)
    {
        m_data = data;

        if (m_constantMovementComponent != null)
        {
            var direction = Vector2Extensions.Random2DVectorIn3D();
            var speed = UnityEngine.Random.Range(m_data.MinSpeed, m_data.MaxSpeed);
            m_constantMovementComponent.Setup(direction , new MovementParameters(), speed);
        }

        if (m_damageComponent != null)
        {
            m_damageComponent.Setup(m_data.Damage , m_data.IgnoreLayers);
        }

        if (m_healthComponent != null)
        {
            m_healthComponent.Setup(m_data.Health, m_data.InvunerableTimeSeconds, lifeTime: m_data.LifeTimeSeconds);
            m_healthComponent.HealthDepleted += OnHealthDepleted;
        }
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
        Game.Instance.CreditPlayerScore(m_data.Score);
        Destroy();
    }
}
