using Unity.VisualScripting;
using UnityEngine;

public struct MovementParameters
{
    public readonly Vector2 Direction;
    public readonly float MinSpeed;
    public readonly float Maxpeed;
    public readonly float Damping;
    public readonly float Acceleration;

    public MovementParameters(Vector2 direction, float maxpeed = 100.0f, float minSpeed = 0f, float damping = 0f, float acceleration = 0f)
    {
        Direction = direction;
        MinSpeed = minSpeed;
        Maxpeed = maxpeed;
        Damping = damping;
        Acceleration = acceleration;
    }
}

public class ConstantMovementComponent : MonoBehaviour
{
    private MovementParameters m_movementParameters;
    private float m_currentSpeed; //current

    public void Setup(MovementParameters movementParameters, float initialSpeed = 0f)
    {
        m_movementParameters = movementParameters;
        m_currentSpeed = initialSpeed;
    }


    private void Update()
    {
        this.transform.position += m_currentSpeed * Time.deltaTime * m_movementParameters.Direction.ToVector3();
    }

}
