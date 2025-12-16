using UnityEngine;

public abstract class SpaceObjectData<T> : ScriptableObject where T : MonoBehaviour
{
    [Header("General")]
    public string Name;
    public T Prefab;

    [Header("Damage")]
    public int Damage = 1;
    public LayerMask IgnoreLayers;
    [Header("Health")]
    public int Health = 1;
    public float InvunerableTimeSeconds = 0.3f;
    public float LifeTimeSeconds = -1f;
    public float HealthRecoveryTime = -1f; // meanign none
}
