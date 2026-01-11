using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSceneReference : MonoBehaviour
{
    [SerializeField] public SpriteRenderer Background;
    [SerializeField] public Vector2 BoundMargin;

    [SerializeField] public PlayerInput PlayerInput;
    [SerializeField] public float DeadZoneAmount;

    [SerializeField] public Transform BulletPoolTransform;
    [SerializeField] public Transform PlayerRoot;
    [SerializeField] public GameObject DebugVectorTarget;

    [SerializeField] public Transform AsteroidPoolTransform;
    [SerializeField] public PooledParticle CollisionParticle;

    [SerializeField] public HUD m_hud;
}
