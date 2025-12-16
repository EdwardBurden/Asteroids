using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    private int m_damage;
    private LayerMask m_ignoreLayers;
    public void Setup(int damage, LayerMask ignoreLayers)
    {
        m_damage = damage;
        m_ignoreLayers = ignoreLayers;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponent<HealthComponent>();
        if (health == null)
            return;

        var ignore = (m_ignoreLayers & (1 << other.gameObject.layer)) != 0;
        if (ignore)
            return;

        health.TakeDamage(m_damage);
    }
}
