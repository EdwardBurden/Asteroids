using UnityEngine;

public class EnemyMovementComponent : ConstantMovementComponent
{
    [SerializeField] private float m_minDistance = 0.3f;
    private Vector3 m_target;

    public override void Setup(Vector2 targetDirection, MovementParameters movementParameters, float initialSpeed = 0)
    {
        base.Setup(targetDirection, movementParameters, initialSpeed);
        SetTargetPosition();
    }

    private void SetTargetPosition()
    {

        m_target = Game.Instance.LevelManager.GetRandomPointInBounds();
    }


    protected override void Update()
    {
        var newPosition = Vector3.Lerp(this.transform.position, m_target, m_currentSpeed * Time.deltaTime);


        //todo min speed clamp
        this.transform.position = newPosition;
        if (Vector3.Distance(this.transform.position, m_target) < m_minDistance)
        {
            SetTargetPosition();
        }
    }
}