using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    private int m_damage;
    public void Setup(int damage) 
    {
        m_damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponent<HealthComponent>();
        if (health == null)
            return;

        health.TakeDamage(m_damage);
    }
}
