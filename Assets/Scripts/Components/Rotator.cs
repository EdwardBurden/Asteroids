using UnityEngine;


public class Rotator : MonoBehaviour
{
    [SerializeField] private float m_initialSpeed;
    [SerializeField] private Vector3 m_rotationAmountsEuler;
    [SerializeField] private float m_damping;

    private float m_rotationSpeed;
    private Vector3 m_rotaionAmountNormalized;

    private void Start()
    {
        SetDirectionAndSpeed(m_initialSpeed, m_rotationAmountsEuler);
    }

    public void SetDirectionAndSpeed(float speed, Vector3 direction) 
    {
        m_rotationSpeed = speed;
        m_rotaionAmountNormalized = direction.normalized;
    }

    private void Update()
    {
        var updateRotation = m_rotaionAmountNormalized * Time.deltaTime * m_rotationSpeed;
        this.transform.Rotate(updateRotation, Space.Self);
        if (m_rotationSpeed - m_damping > 0)
            m_rotationSpeed -= m_damping * Time.deltaTime;
    }
}
