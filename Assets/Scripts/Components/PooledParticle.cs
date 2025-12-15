using UnityEngine;

public class PooledParticle : PooledGameObject
{
    [SerializeField] private ParticleSystem m_particleSystem;

    private void OnEnable()
    {
        m_particleSystem.Play();
    }

    private void Update()
    {
      if(m_particleSystem.is) //here
    }


}
