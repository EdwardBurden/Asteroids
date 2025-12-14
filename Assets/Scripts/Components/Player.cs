using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform m_bulletRoot;

    public Action OnDamaged;
    public Vector3 BulletRoot => m_bulletRoot.position;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Asteroid>() != null)
        {
            OnDamaged?.Invoke();
        }
    }

}
