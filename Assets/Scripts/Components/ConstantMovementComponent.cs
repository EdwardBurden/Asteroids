using Unity.VisualScripting;
using UnityEngine;

public struct MovementParameters
{
    public readonly float MinSpeed;
    public readonly float Maxpeed;
    public readonly float Damping;
    public readonly float Acceleration;

    public MovementParameters( float maxpeed = 100.0f, float minSpeed = 0f, float damping = 0f, float acceleration = 0f)
    {
        MinSpeed = minSpeed;
        Maxpeed = maxpeed;
        Damping = damping;
        Acceleration = acceleration;
    }
}

public class ConstantMovementComponent : MonoBehaviour //todo rename to be more abstarct
{
    protected MovementParameters m_movementParameters;
    protected float m_currentSpeed; //current
    protected Vector3 m_targetDirection;

    public virtual void Setup(Vector2 targetDirection, MovementParameters movementParameters, float initialSpeed = 0f)
    {
        m_targetDirection = targetDirection.ToVector3();
        m_movementParameters = movementParameters;
        m_currentSpeed = initialSpeed;
    }


    protected virtual void Update()
    {
        this.transform.position += m_currentSpeed * Time.deltaTime * m_targetDirection;
    }

}
