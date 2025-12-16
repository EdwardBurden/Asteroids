using System.Collections;
using UnityEngine;

public class PooledParticle : PooledGameObject
{
    [SerializeField] private ParticleSystem m_particleSystem;

    private WaitForSeconds m_aliveTime;

    private void OnEnable()
    {
        m_particleSystem.Play();
        m_aliveTime = new WaitForSeconds(m_particleSystem.main.duration);
        StartCoroutine(CheckAlive());
    }

    private IEnumerator CheckAlive()
    {
        yield return m_aliveTime;
            Destroy();
    }

    private void OnDisable()
    {
        m_particleSystem.Stop();
    }
}
