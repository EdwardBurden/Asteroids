using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMovementComponent : ConstantMovementComponent
{
    private const float MinDirection = 0.05f;
    private Vector3 m_velocity;

    public void SetMovementInput(Vector2 direction) 
    {
        m_targetDirection = direction.ToVector3();
    }

    protected override void Update()
    {
        if (m_targetDirection.sqrMagnitude > MinDirection)
        {
            var direction = m_movementParameters.Acceleration * m_targetDirection;
            m_velocity += direction;

        }
        else
        {
            m_velocity -= m_velocity.normalized * m_movementParameters.Damping;
        }

        m_velocity = Vector2.ClampMagnitude(m_velocity, m_movementParameters.Maxpeed);


        this.transform.position += m_velocity*Time.deltaTime;
    }
}
