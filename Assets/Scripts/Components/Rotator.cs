using UnityEngine;

public sealed class Rotator : MonoBehaviour
{
    private float m_rotationSpeed;
    private Vector3 m_rotaionAmountNormalized;

    private void Start()
    {
        SetDirectionAndSpeed(UnityEngine.Random.Range(1,100), UnityEngine.Random.Range(-10, 10));
    }

    public void SetDirectionAndSpeed(float speed, float spin)
    {
        m_rotationSpeed = speed;
        m_rotaionAmountNormalized = Vector3.forward * spin;
    }

    private void Update()
    {
        var updateRotation = m_rotaionAmountNormalized * Time.deltaTime * m_rotationSpeed;
        this.transform.Rotate(updateRotation, Space.Self);
    }
}
